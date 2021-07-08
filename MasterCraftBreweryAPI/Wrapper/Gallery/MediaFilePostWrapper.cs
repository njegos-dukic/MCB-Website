using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Wrapper
{
    public class MediaFilePostWrapper
    {
        /// <summary>
        /// Image file name and data
        /// </summary>
        public IFormFile PhotoInfo { get; set; }

    }
}
