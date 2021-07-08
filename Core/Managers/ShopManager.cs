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
    public class ShopManager : IShopManager
    {
        private readonly IFileManager fileManager;
        private readonly IApiKeyManager apiKeyManager;
        private readonly IEmailSender emailSender;
        private readonly MasterCraftBreweryContext context;

        public ShopManager(IFileManager fileManager, IApiKeyManager apiKeyManager, IEmailSender emailSender, MasterCraftBreweryContext context)
        {
            this.fileManager = fileManager;
            this.apiKeyManager = apiKeyManager;
            this.emailSender = emailSender;
            this.context = context;
        }

        /// <summary>
        /// To make product available on online shop, this method adds important information 
        /// about the product, for example, the amount that can be ordered, price on online shop etc.
        /// </summary>
        /// <param name="dto">Information about the product requirements for online purchase</param>
        /// <returns></returns>
        public async Task<ResultMessage<bool>> Add(ShopProductServingDTO dto)
        {
            try
            {
                if (await context.ShopProductServings
                                 .AnyAsync(x => x.ProductId == dto.ProductId
                                                && x.ServingId == dto.ServingId))
                    return new ResultMessage<bool>(OperationStatus.Exists);

                ShopProductServing shopProductServing = dto.ToEntity();
                string photoUri = (await context.Products
                                                .Select(x => new { x.PhotoUri, x.ProductId })
                                                .SingleAsync(x => x.ProductId == dto.ProductId))
                                                .PhotoUri;
                shopProductServing.PhotoUri = photoUri;
                await context.ShopProductServings.AddAsync(shopProductServing);
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
        /// Changes image that a product should have on online shop.
        /// </summary>
        /// <param name="shopProductServingId">Unique identifier for the product</param>
        /// <param name="basicFile">Information about the image</param>
        /// <returns></returns>
        public async Task<ResultMessage<bool>> ChangeImage(int shopProductServingId, BasicFileInfo basicFile)
        {
            try
            {
                ShopProductServing entity = await context.ShopProductServings
                                                         .Include(x => x.ShopAmount)
                                                         .SingleOrDefaultAsync(x => x.ShopProductServingId == shopProductServingId);
                if (entity == null)
                    return new ResultMessage<bool>(OperationStatus.NotFound);

                if (await NotAuthenticated(entity))
                    throw new ForbiddenAccessException();

                string relativePathOfImage = PathBuilder.BuildRelativePathForProductImage(basicFile.FileName);
                ResultMessage<bool> savedImage = await fileManager.UploadFile(basicFile.FileData, relativePathOfImage);
                if (!savedImage)
                    return savedImage;

                entity.PhotoUri = relativePathOfImage;
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
        /// Chages status of the order, marking it as delivered or not.
        /// </summary>
        /// <param name="orderId">Unique identifier for the order</param>
        /// <param name="isDelivered">Is the order delievered?</param>
        /// <returns></returns>
        public async Task<ResultMessage<bool>> ChangeStatus(int orderId, bool isDelivered)
        {
            try
            {
                Order order = await context.Orders.SingleOrDefaultAsync(x => x.OrderId == orderId);
                if (order == null)
                    return new ResultMessage<bool>(OperationStatus.NotFound);

                if (!await apiKeyManager.IsRelatedToCompany(order.CompanyId))
                    throw new ForbiddenAccessException();
                order.IsDelivered = isDelivered;
                await context.SaveChangesAsync();

                return new ResultMessage<bool>(true);
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<bool>(status, message);
            }
        }

        /// <summary>
        /// Maps and returns all orderes that are (not) delivered (depending on method's argument 
        /// "isDelivered".
        /// </summary>
        /// <param name="isDelivered">Flag for filtering orders</param>
        /// <returns></returns>
        public async Task<ResultMessage<IAsyncEnumerable<OutputOrderDTO>>> GetOrders(bool? isDelivered)
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();

            Expression<Func<Order, bool>> filter = x => x.CompanyId == companyId;
            if (isDelivered != null)
                filter = x => x.CompanyId == companyId && x.IsDelivered == isDelivered;


            return new ResultMessage<IAsyncEnumerable<OutputOrderDTO>>(
                                GetOrders()
                               .Where(filter)
                               .OrderByDescending(x => x.OrderedOn)
                               .Select(x => x.ToOutputDTO())
                               .AsAsyncEnumerable());
        }

        /// <summary>
        /// Downloads image related to product on online shop.
        /// </summary>
        /// <param name="shopProductServingId">Unique identifier for the product</param>
        /// <param name="thumbnailDimensions">Dimensions for the image</param>
        /// <returns></returns>
        public async Task<ResultMessage<BasicFileInfo>> DownloadImage(int shopProductServingId, ThumbnailDimensions? thumbnailDimensions = null)
        {
            var productImage = await context.ShopProductServings
                                            .Include(x => x.ShopAmount)
                                            .Select(x => new { x.ShopProductServingId, x.PhotoUri, x.ShopAmount.CompanyId })
                                            .SingleOrDefaultAsync(x => x.ShopProductServingId == shopProductServingId);
            if (productImage == null)
                return new ResultMessage<BasicFileInfo>(OperationStatus.NotFound);

            if (!await apiKeyManager.IsRelatedToCompany(productImage.CompanyId))
                throw new ForbiddenAccessException();

            return new ResultMessage<BasicFileInfo>(await fileManager.DownloadFile(productImage.PhotoUri, thumbnailDimensions));
        }

        /// <summary>
        /// Returns information about the products that are related to online shop.
        /// </summary>
        /// <returns></returns>
        public async Task<ResultMessage<IAsyncEnumerable<OutputShopProductServingDTO>>> GetShopProductServings()
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();
            IAsyncEnumerable<OutputShopProductServingDTO> productServings = context.ShopProductServings
                                                                                    .Include(x => x.Product)
                                                                                    .Include(x => x.Serving)
                                                                                    .Include(x => x.ShopAmount)
                                                                                    .Where(x => x.Product.CompanyId == companyId)
                                                                                    .Select(x => x.ToDTO())
                                                                                    .AsAsyncEnumerable();
            return new ResultMessage<IAsyncEnumerable<OutputShopProductServingDTO>>(productServings);
        }

        /// <summary>
        /// Enable user to place an order. If order is valid, it will be saved. 
        /// </summary>
        /// <param name="orderDTO">Information about the order</param>
        /// <returns></returns>
        public async Task<ResultMessage<OutputOrderDTO>> PlaceOrder(OrderDTO orderDTO)
        {
            try
            {
                orderDTO.OrderedOn = DateTime.UtcNow;

                if (!IsValidOrder(orderDTO))
                    return new ResultMessage<OutputOrderDTO>(OperationStatus.InvalidData);

                Order order = orderDTO.ToEntity();
                order.TotalCost = CalculateTotalCost(orderDTO);
                order.CompanyId = await apiKeyManager.GetRelatedCompanyId();
                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();

                OutputOrderDTO addedOrder = (await GetOrders().SingleOrDefaultAsync(x => x.OrderId == order.OrderId))
                                                              .ToOutputDTO();

                // Send an email to notify administrator about new order
                await emailSender.EmailOrder(addedOrder);

                return new ResultMessage<OutputOrderDTO>(addedOrder, OperationStatus.Success);
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<OutputOrderDTO>(status, message);
            }
        }

        private async Task<bool> NotAuthenticated(ShopProductServing entity)
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();
            return entity.ShopAmount.CompanyId != companyId;
        }

        private bool IsValidOrder(OrderDTO order)
        {
            ShopAmount GetRelatedShopConstraints(int shopProductServingId)
                            => context.ShopProductServings
                                      .Include(x => x.ShopAmount)
                                      .FirstOrDefault(x => x.ShopProductServingId == shopProductServingId)
                                      .ShopAmount;

            if (!InputValidator.IsValidEmail(order.Email))
                return false;
            if (!InputValidator.IsValidPhoneNumber(order.PhoneNumber))
                return false;

            return !order.ProductOrders.Select(x => new ProductOrderConstraints(x, GetRelatedShopConstraints(x.ShopProductServingId)))
                                 .GroupBy(x => x.ShopAmount)
                                 .Any(x => x.Any(y => y.OrderedAmount % x.Key.IncrementAmount != 0)
                                           || x.Sum(y => y.OrderedAmount) % x.Key.PackageAmount != 0);
        }

        private double CalculateTotalCost(OrderDTO order)
            => order.ProductOrders.Select(x => x.Price * x.OrderedAmount).Sum() + order.DeliveryCost;

        private IQueryable<Order> GetOrders()
            => context.Orders.Include(x => x.ProductOrders)
                                     .ThenInclude(x => x.ShopProductServing)
                                         .ThenInclude(x => x.Serving)
                               .Include(x => x.ProductOrders)
                                     .ThenInclude(x => x.ShopProductServing)
                                         .ThenInclude(x => x.Product)
                               .Include(x => x.ProductOrders)
                                     .ThenInclude(x => x.ShopProductServing)
                                         .ThenInclude(x => x.ShopAmount);

        class ProductOrderConstraints
        {
            public int ShopProductServingId { get; set; }

            public int OrderedAmount { get; set; }

            public ShopAmount ShopAmount { get; set; }

            public ProductOrderConstraints(ProductOrderDTO productOrder, ShopAmount shopAmount)
            {
                ShopProductServingId = productOrder.ShopProductServingId;
                OrderedAmount = productOrder.OrderedAmount;
                ShopAmount = shopAmount;
            }
        }
    }
}
