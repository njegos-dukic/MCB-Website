using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Wrapper
{
    public class QuoteDeleteWrapper
    {
        /// <summary>
        /// Is quote deleted?
        /// </summary>
        public bool IsDeleted { get; }

        public QuoteDeleteWrapper(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}
