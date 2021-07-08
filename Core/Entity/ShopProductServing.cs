using System.Collections.Generic;

namespace Core.Entity
{
    public class ShopProductServing
    {
        public int ShopProductServingId { get; set; }

        public int ProductId { get; set; }

        public int ServingId { get; set; }

        public int ShopAmountId { get; set; }

        public double Price { get; set; }

        public string PhotoUri { get; set; }

        #region NavigationProperties

        public Product Product { get; set; }

        public ShopAmount ShopAmount { get; set; }

        public Serving Serving { get; set; }

        public ICollection<ProductOrder> ProductOrders { get; set; }
        #endregion
    }
}
