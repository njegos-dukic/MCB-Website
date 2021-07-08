namespace MasterCraftBreweryAPI.Wrapper.Shop
{
    public class ShopProductServingPostWrapper
    {
        /// <summary>
        /// Unique identifier for the related product
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Unique identifier for the related serving size
        /// </summary>
        public int ServingId { get; set; }

        /// <summary>
        /// Unique identifier for the related shop amount
        /// </summary>
        public int ShopAmountId { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public double Price { get; set; }
    }
}
