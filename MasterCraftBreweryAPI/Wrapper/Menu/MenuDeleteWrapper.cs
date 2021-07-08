namespace MasterCraftBreweryAPI.Wrapper.Menu
{
    public class MenuDeleteWrapper
    {
        /// <summary>
        /// Is company deleted?
        /// </summary>
        public bool IsDeleted { get; }

        public MenuDeleteWrapper(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}
