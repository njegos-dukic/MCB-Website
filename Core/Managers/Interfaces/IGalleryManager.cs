#nullable enable
using Core.DTO;
using Core.ErrorHandling;
using Core.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Managers
{
    public interface IGalleryManager
    {
        Task<ResultMessage<GalleryBaseDTO>> Add(GalleryBaseDTO dto);
        public Task<ResultMessage<GalleryDTO>> AddImagesToGallery(int galleryId, List<BasicFileInfo> images);

        Task<ResultMessage<GalleryDTO>> Update(GalleryDTO dto);

        Task<ResultMessage<bool>> Delete(int galleryId);

        Task<ResultMessage<DetailedGalleryDTO>> GetById(int galleryId, ThumbnailDimensions? thumbnailDimensions = null);

        Task<ResultMessage<List<GalleryWithThumbnailDTO>>> GetAll(ThumbnailDimensions? thumbnailDimensions = null);

    }
}
