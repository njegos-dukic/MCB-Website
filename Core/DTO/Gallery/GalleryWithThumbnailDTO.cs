using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class GalleryWithThumbnailDTO : GalleryBaseDTO
    {
        public List<MediaFileDTO> MediaFiles { get; set; }
        
        public DetailedMediaFileDTO Thumbnail { get; set; }
    }
}
