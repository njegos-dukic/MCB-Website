using Core.ErrorHandling;
using Core.Util;
using System.Threading.Tasks;

namespace Core.Managers
{
    public interface IFileManager
    {
        Task<ResultMessage<bool>> UploadFile(byte[] data, string relativePath);

        Task<ResultMessage<BasicFileInfo>> DownloadFile(string relativePath, ThumbnailDimensions thumbnailDimensions = null);

        Task<ResultMessage<bool>> DeleteFile(string relativePath);
    }
}
