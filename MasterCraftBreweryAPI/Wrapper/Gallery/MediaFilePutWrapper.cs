using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Wrapper
{
    public class MediaFilePutWrapper
    {
        /// <summary>
        /// Unique identifier of media file
        /// </summary>
        public int MediaFileId { get; set; }

        /// <summary>
        /// Unique identifier of the gallery that contains media file
        /// </summary>
        public int GalleryId { get; set; }

        /// <summary>
        /// Says if image is thumbnail
        /// </summary>
        public bool IsThumbnail { get; set; }
    }
}
