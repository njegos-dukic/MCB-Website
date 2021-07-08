using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Wrapper
{
    public class QuotePutWrapper
    {
        /// <summary>
        /// Unique identifier for quote
        /// </summary>
        public int QuoteId { get; set; }

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
