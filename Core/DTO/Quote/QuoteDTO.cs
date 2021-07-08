using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class QuoteDTO
    {
        public int QuoteId { get; set; }

        public string QuoteText { get; set; }

        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
