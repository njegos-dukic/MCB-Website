using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Wrapper
{
    public class QuotePostWrapper
    {
        /// <summary>
        /// Text of the quote
        /// </summary>
        public string QuoteText { get; set; }

        /// <summary>
        /// Author of the text quote
        /// </summary>
        public string Author { get; set; }
    }
}
