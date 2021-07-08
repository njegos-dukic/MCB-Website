namespace MasterCraftBreweryAPI.Wrapper
{
    public class AdministratorPostResponseWrapper
    {
        /// <summary>
        /// Is administrator successfully registered?
        /// </summary>
        public bool IsRegistered { get; }

        public AdministratorPostResponseWrapper(bool isRegistered)
        {
            IsRegistered = isRegistered;
        }
    }
}
