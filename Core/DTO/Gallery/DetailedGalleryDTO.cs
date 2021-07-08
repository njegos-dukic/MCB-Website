using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class DetailedGalleryDTO : GalleryBaseDTO
    {
        public List<DetailedMediaFileDTO> MediaFiles { get; set; }
    }
}
