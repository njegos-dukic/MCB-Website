using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class GalleryBaseDTO
    {
        public int GalleryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
