using Core.ErrorHandling;
using Core.Util;
using System.IO;
using System.Threading.Tasks;

namespace Core.Managers
{
    public class LocalFileManager : IFileManager
    {
        private readonly string fileStorageLocation;

        public LocalFileManager(string fileStorageLocation)
            => this.fileStorageLocation = fileStorageLocation;


        public async Task<ResultMessage<BasicFileInfo>> DownloadFile(string relativePath, ThumbnailDimensions thumbnailDimensions = null)
        {
            try
            {
                if (relativePath == null)
                    return new ResultMessage<BasicFileInfo>(OperationStatus.InvalidData);

                if (relativePath == PathBuilder.DefaultProductImage)
                    return DownloadDefaultImage(PathBuilder.DefaultProductImage, thumbnailDimensions);

                string absolutePath = PathBuilder.BuildApsolutePathForFile(fileStorageLocation, relativePath);
                if (!File.Exists(absolutePath))
                    return new ResultMessage<BasicFileInfo>(OperationStatus.NotFound);

                byte[] data = await File.ReadAllBytesAsync(absolutePath);
                byte[] thumbnailData = ImageUtil.GetImageThumbnail(data, Path.GetExtension(relativePath), thumbnailDimensions);
                BasicFileInfo fileInfo = new BasicFileInfo()
                {
                    FileData = thumbnailData,
                    FileName = Path.GetFileName(absolutePath)
                };

                return new ResultMessage<BasicFileInfo>(fileInfo, OperationStatus.Success);
            }
            catch (IOException)
            {
                return new ResultMessage<BasicFileInfo>(OperationStatus.FileSystemError);
            }
        }
       
        public async Task<ResultMessage<bool>> DeleteFile(string relativePath)
        {
            try
            {
                string absolutePath = PathBuilder.BuildApsolutePathForFile(fileStorageLocation, relativePath);
                
                if (!File.Exists(absolutePath))
                    return new ResultMessage<bool>(OperationStatus.NotFound);

                await Task.Run(() => File.Delete(absolutePath));

                return new ResultMessage<bool>(true, OperationStatus.Success);

            }
            catch (IOException ex)
            {
                return new ResultMessage<bool>(OperationStatus.FileSystemError, ex.Message);
            }
        }

        public async Task<ResultMessage<bool>> UploadFile(byte[] data, string relativePath)
        {
            try
            {
                string absolutePath = PathBuilder.BuildApsolutePathForFile(fileStorageLocation, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));
                using FileStream fileStream = File.Create(absolutePath);
                await fileStream.WriteAsync(data);

                return new ResultMessage<bool>(true, OperationStatus.Success);
            }
            catch (IOException ex)
            {
                return new ResultMessage<bool>(OperationStatus.FileSystemError, ex.Message);
            }
        }

        private ResultMessage<BasicFileInfo> DownloadDefaultImage(string resourceName, ThumbnailDimensions thumbnailDimensions)
        {
            byte[] data = StreamUtil.GetManifestResourceBytes(resourceName);
            byte[] thumbnailData = ImageUtil.GetImageThumbnail(data, Path.GetExtension(resourceName), thumbnailDimensions);
            BasicFileInfo fileInfo = new BasicFileInfo()
            {
                FileData = thumbnailData,
                FileName = resourceName
            };
            return new ResultMessage<BasicFileInfo>(fileInfo);
        }
    }
}
