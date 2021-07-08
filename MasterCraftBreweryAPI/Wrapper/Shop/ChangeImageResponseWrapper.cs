namespace MasterCraftBreweryAPI.Wrapper.Shop
{
    public class ChangeImageResponseWrapper
    {
        /// <summary>
        /// Is image changed?
        /// </summary>
        public bool IsChanged { get; }

        public ChangeImageResponseWrapper(bool isChanged)
        {
            IsChanged = isChanged;
        }
    }
}
