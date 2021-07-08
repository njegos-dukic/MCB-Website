using System.Collections.Generic;

namespace Core.Entity
{
    public class ProductType
    {
        public int ProductTypeId { get; set; }

        public string Name { get; set; }

        #region NavigationProperties

        public ICollection<Product> Products { get; set; }

        #endregion
    }
}
