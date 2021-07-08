using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class MediaFileDTO
    {
        public int MediaFileId { get; set; }

        public string Uri { get; set; }

        public string Description { get; set; }

        public int GalleryId { get; set; }

        public bool IsThumbnail { get; set; }
    }
}
