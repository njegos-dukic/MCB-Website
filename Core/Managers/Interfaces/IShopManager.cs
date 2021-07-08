using Core.DTO;
using Core.ErrorHandling;
using Core.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Managers
{
    public interface IShopManager
    {
        Task<ResultMessage<OutputOrderDTO>> PlaceOrder(OrderDTO orderDTO);

        Task<ResultMessage<bool>> ChangeStatus(int orderId, bool isDelivered);

        Task<ResultMessage<bool>> ChangeImage(int shopProductServingId, BasicFileInfo fileInfo);

        Task<ResultMessage<IAsyncEnumerable<OutputOrderDTO>>> GetOrders(bool? isDelivered);

        Task<ResultMessage<IAsyncEnumerable<OutputShopProductServingDTO>>> GetShopProductServings();

        Task<ResultMessage<bool>> Add(ShopProductServingDTO shopProductServing);

        Task<ResultMessage<BasicFileInfo>> DownloadImage(int shopProductServingId, ThumbnailDimensions thumbnailDimensions);
    }
}
