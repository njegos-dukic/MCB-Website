using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    public class Gallery
    {
        public int GalleryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CompanyId { get; set; }

        public DateTime CreatedOn { get; set; }

        #region NavigationProperties

        public Company Company { get; set; }

        public ICollection<MediaFile> MediaFiles { get; set; }

        #endregion
    }
}
