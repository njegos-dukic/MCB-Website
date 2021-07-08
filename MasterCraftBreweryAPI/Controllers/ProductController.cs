#nullable enable
using AutoMapper;
using Core.DTO;
using Core.ErrorHandling;
using Core.Managers;
using Core.Util;
using MasterCraftBreweryAPI.ErrorHandling;
using MasterCraftBreweryAPI.Mapper;
using MasterCraftBreweryAPI.Util;
using MasterCraftBreweryAPI.Wrapper;
using MasterCraftBreweryAPI.Wrapper.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Controllers
{
    [Route("products")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.Authentication.ApiKeyScheme)]
    public class ProductController : MasterCraftBreweryControllerBase
    {
        private readonly IProductManager productManager;

        public ProductController(IProductManager productManager, IApiKeyManager apiKeyManager, IMapper mapper) : base(apiKeyManager, mapper)
            => this.productManager = productManager;

        /// <summary>
        /// Adds new product into the database if there is no such product already 
        /// saved. It relates product with a certain product type and with different serving options.
        /// </summary>
        /// <param name="product">The product information with serving options and 
        /// unique identificator for the product type</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Add([FromBody] ProductPostWrapper product)
        {
            ResultMessage<OutputProductDTO> result = await productManager.Add(mapper.Map<InputProductDTO>(product));
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Updates the whole product object with product type and serving options. 
        /// Product needs to be identified using its unique identifier.
        /// If there is no such product in the database, OperationStatus.NotFound is returned.
        /// </summary>
        /// <param name="product">New product information with already existing unique identifier</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Update([FromBody] ProductPutWrapper product)
        {
            ResultMessage<OutputProductDTO> result = await productManager.Update(mapper.Map<InputProductDTO>(product));
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Completely deletes a product from the database, based on specific unique identifier.
        /// </summary>
        /// <param name="productId">Unique identifier for the product</param>
        /// <returns>True if deleted, false if not</returns>
        [HttpDelete("{productId}")]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Delete([FromRoute] int productId)
        {
            ResultMessage<bool> result = await productManager.Delete(productId);
            return HttpResultMessage.FilteredResult<ProductDeleteWrapper, bool>(result);
        }

        /// <summary>
        /// Finds and returns a specific product from database, 
        /// with possible servings and product type.
        /// </summary>
        /// <param name="productId">Unique identifier for the product</param>
        /// <returns></returns>
        [HttpGet("{productId}")]
        public async Task<ActionResult> GetById([FromRoute] int productId)
        {
            ResultMessage<OutputProductDTO> result = await productManager.GetById(productId);
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Returns all current products from database.
        /// If productTypeName is specified, then returns all products from database that belong to the same product category (product type).
        /// </summary>
        /// <paramref name="productTypeName">Unique name identifier for the product type (optional)</paramref>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] string? productTypeName = null)
        {
            ResultMessage<IAsyncEnumerable<OutputProductDTO>> result = await productManager.GetAll(productTypeName);
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Returns all current product servings related to a company from database.
        /// </summary>
        /// <returns></returns>
        [HttpGet("product-servings")]
        public async Task<ActionResult> GetAllProductServings()
        {
            ResultMessage<IAsyncEnumerable<DetailedProductServingDTO>> result = await productManager.GetProductServings();
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Changes image of an product.
        /// </summary>
        /// <param name="productId">Unique identifier for the product</param>
        /// <param name="file">Information about the file: filename and data</param>
        /// <returns></returns>
        [HttpPut("/{productId}/image")]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> ChangeImage([FromRoute] int productId, IFormFile file)
        {
            ResultMessage<bool> result = await productManager.ChangeImage(productId, file.AsBasicFileInfo());
            return HttpResultMessage.FilteredResult<ChangeImageResponseWrapper, bool>(result);
        }

        /// <summary>
        /// Downloads image for specified product, if such product exists. If product does not exist,
        /// method returns no content. If specified image is not found or it is not set, method returns null.
        /// </summary>
        /// <param name="productId">Unique identifier for the product</param>
        /// <param name="dimensions">Dimensions for the product's image</param>
        /// <returns></returns>
        [HttpGet("{productId}/image")]
        public async Task<ActionResult> DownloadImage([FromRoute] int productId, [FromQuery] ThumbnailDimensionsWrapper dimensions)
        {
            ResultMessage<BasicFileInfo> result = await productManager.DownloadImage(productId, dimensions?.ToCoreObject(mapper));
            return HttpResultMessage.FilteredResult(result, ContentType.File);
        }
    }
}
