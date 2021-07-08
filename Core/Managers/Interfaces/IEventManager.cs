#nullable enable
using Core.DTO;
using Core.ErrorHandling;
using Core.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Managers
{
    public interface IEventManager
    {
        Task<ResultMessage<EventDTO>> Add(EventDTO eventDTO);

        Task<ResultMessage<EventDTO>> Update(EventDTO eventDTO);

        Task<ResultMessage<bool>> ChangeImage(int eventId, BasicFileInfo basicFile);

        Task<ResultMessage<BasicFileInfo>> DownloadImage(int productId, ThumbnailDimensions? thumbnailDimensions = null);

        Task<ResultMessage<bool>> Delete(int eventId);

        Task<ResultMessage<EventDTO>> GetById(int eventId);

        Task<ResultMessage<IAsyncEnumerable<EventDTO>>> GetAll();
    }
}
