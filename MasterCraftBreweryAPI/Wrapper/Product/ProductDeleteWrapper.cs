namespace MasterCraftBreweryAPI.Wrapper.Product
{
    public class ProductDeleteWrapper
    {
        /// <summary>
        /// Is company deleted?
        /// </summary>
        public bool IsDeleted { get; }

        public ProductDeleteWrapper(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}
