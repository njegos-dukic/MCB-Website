namespace MasterCraftBreweryAPI.Wrapper.Shop
{
    public class IsShopProductServingDeletedResponseWrapper
    {
        /// <summary>
        /// Is deleted?
        /// </summary>
        public bool IsDeleted { get; }

        public IsShopProductServingDeletedResponseWrapper(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}
