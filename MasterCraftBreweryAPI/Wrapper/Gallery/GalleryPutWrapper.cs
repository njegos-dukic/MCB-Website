using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Wrapper
{
    public class GalleryPutWrapper
    {
        /// <summary>
        /// Unique identifier of the gallery
        /// </summary>
        public int GalleryId { get; set; }

        /// <summary>
        /// Gallery name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gallery description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gallery images
        /// </summary>
        public List<MediaFilePutWrapper> MediaFiles { get; set; }
    }
}
