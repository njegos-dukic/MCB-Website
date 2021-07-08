#nullable enable
using Core.DTO;
using Core.ErrorHandling;
using Core.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Managers
{
    public interface IProductManager
    {
        Task<ResultMessage<OutputProductDTO>> Add(InputProductDTO dto);

        Task<ResultMessage<OutputProductDTO>> Update(InputProductDTO dto);

        Task<ResultMessage<bool>> Delete(int productId);

        Task<ResultMessage<OutputProductDTO>> GetById(int productId);

        Task<ResultMessage<IAsyncEnumerable<OutputProductDTO>>> GetAll(string? productTypeName = null);

        Task<ResultMessage<IAsyncEnumerable<DetailedProductServingDTO>>> GetProductServings();

        Task<ResultMessage<bool>> ChangeImage(int productId, BasicFileInfo basicFile);

        Task<ResultMessage<BasicFileInfo>> DownloadImage(int productId, ThumbnailDimensions? thumbnailDimensions = null);
    }
}
