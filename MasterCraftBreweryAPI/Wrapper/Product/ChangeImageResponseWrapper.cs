namespace MasterCraftBreweryAPI.Wrapper.Product
{
    public class ChangeImageResponseWrapper
    {
        /// <summary>
        /// Is company deleted?
        /// </summary>
        public bool IsChanged { get; }

        public ChangeImageResponseWrapper(bool isChanged)
        {
            IsChanged = isChanged;
        }
    }
}
