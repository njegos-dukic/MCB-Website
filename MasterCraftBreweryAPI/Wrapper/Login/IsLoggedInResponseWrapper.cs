namespace MasterCraftBreweryAPI.Wrapper
{
    public class IsLoggedInResponseWrapper
    {
        /// <summary>
        /// Is user logged in?
        /// </summary>
        public bool IsLoggedIn { get; }

        public IsLoggedInResponseWrapper(bool isLoggedIn)
        {
            IsLoggedIn = isLoggedIn;
        }
    }
}
