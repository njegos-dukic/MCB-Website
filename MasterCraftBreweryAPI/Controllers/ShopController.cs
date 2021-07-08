using AutoMapper;
using Core.DTO;
using Core.ErrorHandling;
using Core.Managers;
using Core.Util;
using MasterCraftBreweryAPI.ErrorHandling;
using MasterCraftBreweryAPI.Mapper;
using MasterCraftBreweryAPI.Util;
using MasterCraftBreweryAPI.Wrapper;
using MasterCraftBreweryAPI.Wrapper.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Controllers
{
    [Route("shop")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.Authentication.ApiKeyScheme)]
    public class ShopController : MasterCraftBreweryControllerBase
    {
        private readonly IShopManager shopManager;

        public ShopController(IShopManager shopManager, IApiKeyManager apiKeyManager, IMapper mapper) : base(apiKeyManager, mapper)
            => this.shopManager = shopManager;

        /// <summary>
        /// To make product available on online shop, this method adds important information 
        /// about the product, for example, the amount that can be ordered, price on online shop etc.
        /// </summary>
        /// <param name="wrapper">Information about the product requirements for online purchase</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Add([FromBody] ShopProductServingPostWrapper wrapper)
        {
            ResultMessage<bool> isAdded = await shopManager.Add(mapper.Map<ShopProductServingDTO>(wrapper));
            return HttpResultMessage.FilteredResult<AddedReponseWrapper, bool>(isAdded);
        }

        /// <summary>
        /// Returns information about the products that are related to online shop.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetShopProductServings()
        {
            ResultMessage<IAsyncEnumerable<OutputShopProductServingDTO>> dtos = await shopManager.GetShopProductServings();
            return HttpResultMessage.FilteredResult(dtos);
        }

        /// <summary>
        /// Changes image that a product should have on online shop.
        /// </summary>
        /// <param name="shopProductServingId">Unique identifier for the product</param>
        /// <param name="file">Information about the image</param>
        /// <returns></returns>
        [HttpPut("{shopProductServingId}/image")]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> ChangeImage([FromRoute] int shopProductServingId, IFormFile file)
        {
            ResultMessage<bool> result = await shopManager.ChangeImage(shopProductServingId, file.AsBasicFileInfo());
            return HttpResultMessage.FilteredResult<ChangeImageResponseWrapper, bool>(result);
        }

        /// <summary>
        /// Downloads image related to product on online shop.
        /// </summary>
        /// <param name="shopProductServingId">Unique identifier for the product</param>
        /// <param name="dimensions">Dimensions for the image</param>
        /// <returns></returns>
        [HttpGet("{shopProductServingId}/image")]
        public async Task<ActionResult> DownloadImage([FromRoute] int shopProductServingId, [FromQuery] ThumbnailDimensionsWrapper dimensions)
        {
            ResultMessage<BasicFileInfo> result = await shopManager.DownloadImage(shopProductServingId, dimensions?.ToCoreObject(mapper));
            return HttpResultMessage.FilteredResult(result, ContentType.File);
        }

        /// <summary>
        /// Enable user to place an order. If order is valid, it will be saved. 
        /// </summary>
        /// <param name="wrapper">Information about the order</param>
        /// <returns></returns>
        [HttpPost("orders")]
        public async Task<ActionResult> PlaceOrder([FromBody] OrderPostWrapper wrapper)
        {
            ResultMessage<OutputOrderDTO> order = await shopManager.PlaceOrder(mapper.Map<OrderDTO>(wrapper));
            return HttpResultMessage.FilteredResult(order);
        }

        /// <summary>
        /// Chages status of the order, marking it as delivered or not.
        /// </summary>
        /// <param name="orderId">Unique identifier for the order</param>
        /// <param name="isDelivered">Is the order delievered?</param>
        /// <returns></returns>
        [HttpPut("orders/{orderId}")]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> ChangeOrderStatus([FromRoute] int orderId, [FromQuery] bool isDelivered)
        {
            ResultMessage<bool> changed = await shopManager.ChangeStatus(orderId, isDelivered);
            return HttpResultMessage.FilteredResult(changed);
        }

        /// <summary>
        /// Maps and returns all orderes that are (not) delivered (depending on method's argument 
        /// "isDelivered".
        /// </summary>
        /// <param name="isDelivered">Flag for filtering orders</param>
        /// <returns></returns>
        [HttpGet("orders")]
        public async Task<ActionResult> GetOrders([FromQuery] bool? isDelivered = null)
        {
            ResultMessage<IAsyncEnumerable<OutputOrderDTO>> orders = await shopManager.GetOrders(isDelivered);
            return HttpResultMessage.FilteredResult(orders);
        }
    }
}