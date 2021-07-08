namespace MasterCraftBreweryAPI.Wrapper.Contact
{
    public class ContactMessagePostResponseWrapper
    {
        /// <summary>
        /// Is message sent?
        /// </summary>
        public bool IsSent { get; }

        public ContactMessagePostResponseWrapper(bool isSent)
        {
            IsSent = isSent;
        }
    }
}
