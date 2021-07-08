namespace MasterCraftBreweryAPI.Wrapper.Shop
{
    public class ProductOrderWrapper
    {
        /// <summary>
        /// Unique identifier for a product serving
        /// </summary>
        public int ShopProductServingId { get; set; }

        /// <summary>
        /// Total price
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Number of same products
        /// </summary>
        public int OrderedAmount { get; set; }
    }
}
