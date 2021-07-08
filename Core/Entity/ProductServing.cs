using System.Collections.Generic;

namespace Core.Entity
{
    public class ProductServing
    {
        public int ProductServingId { get; set; }

        public int ProductId { get; set; }

        public int ServingId { get; set; }

        public double Price { get; set; }

        #region NavigationProperties
        public Product Product { get; set; }

        public Serving Serving { get; set; }

        public ICollection<MenuItem> menuItems { get; set; }
        #endregion
    }
}
