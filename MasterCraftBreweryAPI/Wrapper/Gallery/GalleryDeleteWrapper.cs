using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Wrapper
{
    public class GalleryDeleteWrapper
    {
        /// <summary>
        /// Is gallery deleted?
        /// </summary>
        public bool IsDeleted { get; }

        public GalleryDeleteWrapper(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}
