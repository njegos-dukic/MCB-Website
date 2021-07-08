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
using System.Text;
using System.Threading.Tasks;

namespace Core.Managers
{
    public class GalleryManager : IGalleryManager
    {
        private readonly IFileManager fileManager;
        private readonly IApiKeyManager apiKeyManager;
        private readonly MasterCraftBreweryContext context;

        public GalleryManager(IFileManager fileManager, IApiKeyManager apiKeyManager, MasterCraftBreweryContext context)
        => (this.fileManager, this.apiKeyManager, this.context) = (fileManager, apiKeyManager, context);

        /// <summary>
        /// Adds new gallery to the database, including mediafiles.
        /// </summary>
        /// <param name="galleryDTO">Gallery data transfer object</param>
        /// <returns></returns>
        public async Task<ResultMessage<GalleryBaseDTO>> Add(GalleryBaseDTO galleryDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(galleryDTO.Name))
                    return new ResultMessage<GalleryBaseDTO>(OperationStatus.InvalidData);

                if (await context.Galleries.AnyAsync(g => g.Name == galleryDTO.Name))
                    return new ResultMessage<GalleryBaseDTO>(OperationStatus.Exists);

                    Gallery gallery = galleryDTO.ToEntity();
                gallery.CompanyId = await apiKeyManager.GetRelatedCompanyId();
                gallery.CreatedOn = DateTime.UtcNow;
                await context.AddAsync(gallery);
                await context.SaveChangesAsync();


                return new ResultMessage<GalleryBaseDTO>(gallery.ToGalleryBaseDTO());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<GalleryBaseDTO>(status, message);
            }
        }

        /// <summary>
        /// Adds images to the gallery
        /// </summary>
        /// <param name="galleryId">Unique identifier of gallery</param>
        /// <param name="images">Image files that will be added to gallery </param>
        /// <returns></returns>
        public async Task<ResultMessage<GalleryDTO>> AddImagesToGallery(int galleryId, List<BasicFileInfo> images)
        {
            try
            {
                Gallery gallery = await context.Galleries.Include(g => g.MediaFiles)
                                                .SingleOrDefaultAsync(g => g.GalleryId == galleryId);

                if (gallery == null)
                    return new ResultMessage<GalleryDTO>(OperationStatus.NotFound);

                if (await NotAuthenticated(gallery))
                    throw new ForbiddenAccessException();

                foreach (BasicFileInfo image in images)
                    await UploadImage(galleryId, image);

                return new ResultMessage<GalleryDTO>(gallery.ToGalleryDTO());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<GalleryDTO>(status, message);
            }
        }

        /// <summary>
        /// Updates gallery. If gallery with specific GalleryId does not exist 
        /// method returns OperationStatus.NotFound.
        /// MediaFiles will be deleted if does not exist in galleryDTO. 
        /// </summary>
        /// <param name="galleryDTO">Gallery data transfer object</param>
        /// <returns></returns>
        public async Task<ResultMessage<GalleryDTO>> Update(GalleryDTO galleryDTO)
        {
            try
            {
                Gallery gallery = await context.Galleries.Include(g => g.MediaFiles)
                                        .SingleOrDefaultAsync(g => g.GalleryId == galleryDTO.GalleryId);

                if (gallery == null)
                    return new ResultMessage<GalleryDTO>(OperationStatus.NotFound);

                if (await NotAuthenticated(gallery))
                    throw new ForbiddenAccessException();

                GalleryDTO galleryDTO1 = gallery.ToGalleryDTO();

                foreach (MediaFileDTO oldDTO in galleryDTO1.MediaFiles)
                {
                    bool found = galleryDTO.MediaFiles.Any(mf => mf.MediaFileId == oldDTO.MediaFileId);
                    if (!found)
                        await DeleteMediaFile(oldDTO.MediaFileId);
                }

                gallery.Name = galleryDTO.Name;
                gallery.Description = galleryDTO.Description;
                await context.SaveChangesAsync();

                return new ResultMessage<GalleryDTO>(gallery.ToGalleryDTO());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<GalleryDTO>(status, message);
            }
        }

        private async Task<bool> DeleteMediaFile(int mediaFileId)
        {
            try
            {
                MediaFile mediaFile = await context.MediaFiles.SingleOrDefaultAsync(mf => mf.MediaFileId == mediaFileId);

                if (mediaFile == null)
                    return false;

                await DeleteImage(mediaFile.Uri);

                context.MediaFiles.Remove(mediaFile);
                await context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<bool>(status, message);
            }
        }


        /// <summary>
        /// Deletes gallery with mediafiles from database.
        /// </summary>
        /// <param name="galleryId">Unique identifier of gallery</param>
        /// <returns></returns>
        public async Task<ResultMessage<bool>> Delete(int galleryId)
        {
            try
            {
                Gallery gallery = await context.Galleries.Include(g => g.MediaFiles)
                                                    .SingleOrDefaultAsync(g => g.GalleryId == galleryId);

                if (gallery == null)
                    return new ResultMessage<bool>(OperationStatus.NotFound);

                if (await NotAuthenticated(gallery))
                    throw new ForbiddenAccessException();

                GalleryDTO galleryDTO = gallery.ToGalleryDTO();
                galleryDTO.MediaFiles.ForEach(async mf => await DeleteImage(mf.Uri));

                context.Galleries.Remove(gallery);
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
        /// Returns list of galleries with thumbnail images.
        /// </summary>
        /// <param name="thumbnailDimensions">Thumbnail dimension</param>
        /// <returns></returns>
        public async Task<ResultMessage<List<GalleryWithThumbnailDTO>>> GetAll(ThumbnailDimensions? thumbnailDimensions = null)
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();
            IQueryable<Gallery> galleries = context.Galleries.Include(g => g.MediaFiles)
                                                                    .Where(g => g.CompanyId == companyId)
                                                                    .OrderByDescending(x => x.CreatedOn);

            List<GalleryWithThumbnailDTO> galleryList = await galleries.Select(g => g.ToThumbnailGalleryDTO()).ToListAsync();

            foreach (GalleryWithThumbnailDTO gallery in galleryList)
            {
                DetailedMediaFileDTO thumbnail = gallery.MediaFiles.FirstOrDefault().ToDetailedDTO();
                if (thumbnail != null)
                {
                    thumbnail.PhotoInfo = await DownloadImage(thumbnail.MediaFileId, thumbnailDimensions);
                    gallery.Thumbnail = thumbnail;
                }
            }

            return new ResultMessage<List<GalleryWithThumbnailDTO>>(galleryList);
        }

        /// <summary>
        /// Returns gallery by unique identifier. 
        /// Also returns all mediafiles with image file data.
        /// </summary>
        /// <param name="galleryId">Gallery unique identifier</param>
        /// <param name="thumbnailDimensions">Thumbnail dimension</param>
        /// <returns></returns>
        public async Task<ResultMessage<DetailedGalleryDTO>> GetById(int galleryId, ThumbnailDimensions? thumbnailDimensions = null)
        {
            Gallery gallery = await context.Galleries.Include(g => g.MediaFiles)
                                                    .SingleOrDefaultAsync(g => g.GalleryId == galleryId);

            if (gallery == null)
                return new ResultMessage<DetailedGalleryDTO>(OperationStatus.NotFound);

            if (await NotAuthenticated(gallery))
                throw new ForbiddenAccessException();

            DetailedGalleryDTO galleryDTO = gallery.ToDetailedDTO();
            foreach (DetailedMediaFileDTO mf in galleryDTO.MediaFiles)
            {
                mf.PhotoInfo = await DownloadImage(mf.MediaFileId, thumbnailDimensions);
            }

            return new ResultMessage<DetailedGalleryDTO>(galleryDTO);
        }


        private async Task<ResultMessage<bool>> UploadImage(int galleryId, BasicFileInfo basicFileInfo)
        {
            try
            {
                Gallery gallery = await context.Galleries.SingleOrDefaultAsync(g => g.GalleryId == galleryId);

                if (gallery == null)
                    return new ResultMessage<bool>(OperationStatus.NotFound);

                if (await NotAuthenticated(gallery))
                    throw new ForbiddenAccessException();

                string relativeImagePath = PathBuilder.BuildRelativePathOfMediaFileImage(basicFileInfo.FileName);
                ResultMessage<bool> savedImage = await fileManager.UploadFile(basicFileInfo.FileData, relativeImagePath);

                if (!savedImage)
                    return savedImage;
                MediaFile mediaFile = new MediaFile()
                {
                    GalleryId = galleryId,
                    Uri = relativeImagePath
                };
                await context.AddAsync(mediaFile);
                await context.SaveChangesAsync();
                return new ResultMessage<bool>(true, OperationStatus.Success);
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<bool>(status, message);
            }
        }

        private async Task<ResultMessage<BasicFileInfo>> DownloadImage(int mediaFileId, ThumbnailDimensions? thumbnailDimensions = null)
        {
            MediaFile mediaFileImage = await context.MediaFiles.SingleOrDefaultAsync(mf => mf.MediaFileId == mediaFileId);

            if (mediaFileImage == null)
                return new ResultMessage<BasicFileInfo>(OperationStatus.NotFound);

            return new ResultMessage<BasicFileInfo>(await fileManager.DownloadFile(mediaFileImage.Uri, thumbnailDimensions));
        }


        private async Task<ResultMessage<bool>> DeleteImage(string mediaFileUri)
            => await fileManager.DeleteFile(mediaFileUri);

        private async Task<bool> NotAuthenticated(Gallery gallery)
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();
            return companyId != gallery.CompanyId;
        }
    }
}
