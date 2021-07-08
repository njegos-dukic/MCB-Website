using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Wrapper
{
    public class GalleryPostWrapper
    {
        /// <summary>
        /// Name of the gallery
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the gallery
        /// </summary>
        public string Description { get; set; }

        ///// <summary>
        ///// Gallery images
        ///// </summary>
        //public List<IFormFile> Images { get; set; }
    }
}
