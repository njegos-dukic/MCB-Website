using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class GalleryDTO : GalleryBaseDTO
    {
        public List<MediaFileDTO> MediaFiles { get; set; }
    }
}
