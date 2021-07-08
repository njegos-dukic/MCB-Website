using System.Collections.Generic;

namespace Core.Entity
{
    public class ShopAmount
    {
        public int ShopAmountId { get; set; }

        public int PackageAmount { get; set; }

        public int IncrementAmount { get; set; }

        public int CompanyId { get; set; }

        #region NavigationProperties

        public ICollection<ShopProductServing> ShopProductServings { get; set; }

        public Company Company { get; set; }
        #endregion
    }
}
