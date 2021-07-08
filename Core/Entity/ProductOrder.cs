namespace Core.Entity
{
    public class ProductOrder
    {
        public int ProductOrderId { get; set; }

        public int ShopProductServingId { get; set; }

        public int OrderId { get; set; }

        public double Price { get; set; }

        public int TotalAmount { get; set; }

        #region NavigationProperties
        public Order Order { get; set; }

        public ShopProductServing ShopProductServing { get; set; }
        #endregion
    }
}
