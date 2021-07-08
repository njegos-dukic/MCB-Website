namespace MasterCraftBreweryAPI.Wrapper
{
    public class ProductServingPostWrapper
    {
        /// <summary>
        /// Unique identifier for the serving related to the product
        /// </summary>
        public int ServingId { get; set; }

        /// <summary>
        /// Price of the product's serving
        /// </summary>
        public double Price { get; set; }
    }
}
