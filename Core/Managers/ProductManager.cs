#nullable enable
using Core.AutoMapper.ExtensionMethods;
using Core.DTO;
using Core.Entity;
using Core.ErrorHandling;
using Core.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Managers
{
    public class ProductManager : IProductManager
    {
        private readonly IFileManager fileManager;
        private readonly IApiKeyManager apiKeyManager;
        private readonly MasterCraftBreweryContext context;

        public ProductManager(IFileManager fileManager, IApiKeyManager apiKeyManager, MasterCraftBreweryContext context)
            => (this.fileManager, this.apiKeyManager, this.context) = (fileManager, apiKeyManager, context);

        /// <summary>
        /// Adds new product into the database if there is no such product already 
        /// saved. It relates product with a certain product type and with different serving options.
        /// </summary>
        /// <param name="dto">The product information with serving options and 
        /// unique identificator for the product type</param>
        /// <returns></returns>
        public async Task<ResultMessage<OutputProductDTO>> Add(InputProductDTO dto)
        {
            try
            {
                if (await context.Products.AnyAsync(x => x.Name == dto.Name))
                    return new ResultMessage<OutputProductDTO>(OperationStatus.Exists);

                Product product = dto.ToEntity();
                product.CompanyId = await apiKeyManager.GetRelatedCompanyId();
                await context.AddAsync(product);
                await context.SaveChangesAsync();

                return new ResultMessage<OutputProductDTO>(product.ToDTO());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<OutputProductDTO>(status, message);
            }
        }

        /// <summary>
        /// Updates the whole product object with product type and serving options. 
        /// Product needs to be identified using its unique identifier.
        /// If there is no such product in the database, OperationStatus.NotFound is returned.
        /// </summary>
        /// <param name="dto">New product information with already existing unique identifier</param>
        /// <returns></returns>
        public async Task<ResultMessage<OutputProductDTO>> Update(InputProductDTO dto)
        {
            try
            {
                Product product = await context.Products.Include(x => x.ProductServings)
                                                        .Include(x => x.Ingredients)
                                                        .SingleOrDefaultAsync(x => x.ProductId == dto.ProductId);
                if (product == null)
                    return new ResultMessage<OutputProductDTO>(OperationStatus.NotFound);

                if (await NotAuthenticated(dto.ProductId))
                    throw new ForbiddenAccessException();

                Product newProduct = dto.ToEntity();
                newProduct.CompanyId = product.CompanyId;
                product.ProductServings = newProduct.ProductServings;
                product.Ingredients = newProduct.Ingredients;
                context.Entry(product).CurrentValues.SetValues(newProduct);
                await context.SaveChangesAsync();

                return new ResultMessage<OutputProductDTO>(product.ToDTO());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<OutputProductDTO>(status, message);
            }
        }

        /// <summary>
        /// Completely deletes a product from the database, based on specific unique identifier.
        /// </summary>
        /// <param name="productId">Unique identifier for the product</param>
        /// <returns>True if deleted, false if not</returns>
        public async Task<ResultMessage<bool>> Delete(int productId)
        {
            try
            {
                Product product = await GetByFilter(x => x.ProductId == productId);
                if (product == null)
                    return new ResultMessage<bool>(OperationStatus.NotFound);

                if (await NotAuthenticated(productId))
                    throw new ForbiddenAccessException();

                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return new ResultMessage<bool>(true, OperationStatus.Success);
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<bool>(status, message);
            }
        }

        /// <summary>
        /// Returns all current products from database.
        /// If productTypeName is specified, then returns all products from database that belong to the same product category (product type).
        /// </summary>
        /// <paramref name="productTypeName">Unique name for the product type</paramref>
        /// <returns></returns>
        public async Task<ResultMessage<IAsyncEnumerable<OutputProductDTO>>> GetAll(string? productTypeName = null)
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();

            Expression<Func<Product, bool>> filter = x => x.CompanyId == companyId;
            if (productTypeName != null)
                filter = x => x.CompanyId == companyId
                && x.ProductType.Name == productTypeName;

            return new ResultMessage<IAsyncEnumerable<OutputProductDTO>>(GetManyByFilter(filter));
        }

        /// <summary>
        /// Returns product servings related to the company.
        /// </summary>
        /// <returns></returns>
        public async Task<ResultMessage<IAsyncEnumerable<DetailedProductServingDTO>>> GetProductServings()
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();

            return new ResultMessage<IAsyncEnumerable<DetailedProductServingDTO>>(context.ProductServings.Include(x => x.Serving)
                                    .Include(x => x.Product)
                                    .Where(x => x.Product.CompanyId == companyId)
                                    .Select(x => x.ToDTO())
                                    .AsAsyncEnumerable());
        }

        /// <summary>
        /// Finds and returns a specific product from database, 
        /// with possible servings and product type.
        /// </summary>
        /// <param name="productId">Unique identifier for the product</param>
        /// <returns>Found product or null</returns>
        public async Task<ResultMessage<OutputProductDTO>> GetById(int productId)
        {
            Product product = await GetByFilter(x => x.ProductId == productId);
            if (product == null)
                return new ResultMessage<OutputProductDTO>(OperationStatus.NotFound);

            if (await NotAuthenticated(productId))
                throw new ForbiddenAccessException();

            OutputProductDTO dto = product.ToDTO();
            return new ResultMessage<OutputProductDTO>(dto);
        }

        /// <summary>
        /// Enables user to change image that represents certain product.
        /// </summary>
        /// <param name="productId">Unique identifier for the product</param>
        /// <param name="basicFile">Image's data</param>
        /// <returns></returns>
        public async Task<ResultMessage<bool>> ChangeImage(int productId, BasicFileInfo basicFile)
        {
            try
            {
                Product product = await GetByFilter(x => x.ProductId == productId);
                if (product == null)
                    return new ResultMessage<bool>(OperationStatus.NotFound);

                if (await NotAuthenticated(productId))
                    throw new ForbiddenAccessException();

                string relativePathOfImage = PathBuilder.BuildRelativePathForProductImage(basicFile.FileName);
                ResultMessage<bool> savedImage = await fileManager.UploadFile(basicFile.FileData, relativePathOfImage);
                if (!savedImage)
                    return savedImage;

                product.PhotoUri = relativePathOfImage;
                await context.SaveChangesAsync();
                return new ResultMessage<bool>(true, OperationStatus.Success);
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<bool>(status, message);
            }
        }

        public async Task<ResultMessage<BasicFileInfo>> DownloadImage(int productId, ThumbnailDimensions? thumbnailDimensions = null)
        {
            var productImage = await context.Products
                                              .Select(x => new { x.ProductId, x.PhotoUri })
                                              .SingleOrDefaultAsync(x => x.ProductId == productId);
            if (productImage == null)
                return new ResultMessage<BasicFileInfo>(OperationStatus.NotFound);

            if (await NotAuthenticated(productId))
                throw new ForbiddenAccessException();

            return new ResultMessage<BasicFileInfo>(await GetImage(productImage.PhotoUri, thumbnailDimensions));
        }

        private async Task<Product> GetByFilter(Expression<Func<Product, bool>> filter)
            => await context.Products.Include(x => x.ProductServings)
                                     .ThenInclude(x => x.Serving)
                                     .Include(x => x.ProductType)
                                     .Include(x => x.Ingredients)
                                     .SingleOrDefaultAsync(filter);

        private IAsyncEnumerable<OutputProductDTO> GetManyByFilter(Expression<Func<Product, bool>>? filter = null)
        {
            IQueryable<Product> products = context.Products.Include(x => x.ProductServings)
                                                           .ThenInclude(x => x.Serving)
                                                           .Include(x => x.ProductType)
                                                           .Include(x => x.Ingredients);
            if (filter != null)
                products = products.Where(filter);
            return products.Select(x => x.ToDTO()).AsAsyncEnumerable();
        }

        private async Task<BasicFileInfo> GetImage(string photoUri, ThumbnailDimensions? thumbnailDimensions = null)
            => await fileManager.DownloadFile(photoUri, thumbnailDimensions);

        private async Task<bool> NotAuthenticated(int productId)
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();
            return !await context.Products.AnyAsync(x => x.ProductId == productId && x.CompanyId == companyId);
        }
    }
}