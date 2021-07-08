using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    public class Quote
    {
        public int QuoteId { get; set; }

        public string QuoteText { get; set; }

        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CompanyId { get; set; }


        #region NavigationProperties
        
        public Company Company { get; set; }

        #endregion
    }
}
