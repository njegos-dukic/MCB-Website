namespace MasterCraftBreweryAPI.Wrapper.Event
{
    public class ChangeImageResponseWrapper
    {
        /// <summary>
        /// Is the image for the event changed?
        /// </summary>
        public bool IsChanged { get; }

        public ChangeImageResponseWrapper(bool isChanged)
        {
            IsChanged = isChanged;
        }
    }
}
