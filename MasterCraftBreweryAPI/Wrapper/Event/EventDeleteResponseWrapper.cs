namespace MasterCraftBreweryAPI.Wrapper.Event
{
    public class EventDeleteResponseWrapper
    {
        /// <summary>
        /// Is the event deleted?
        /// </summary>
        public bool IsDeleted { get; }

        public EventDeleteResponseWrapper(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}
