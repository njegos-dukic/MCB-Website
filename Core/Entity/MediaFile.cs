using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    public class MediaFile
    {
        public int MediaFileId { get; set; }

        public string Uri { get; set; }

        public int GalleryId { get; set; }

        public bool IsThumbnail { get; set; }

        #region NavigationProperties

        public Gallery Gallery { get; set; }

        #endregion
    }
}
