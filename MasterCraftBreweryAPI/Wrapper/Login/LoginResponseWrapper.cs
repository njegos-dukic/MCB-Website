namespace MasterCraftBreweryAPI.Wrapper
{
    public class LoginResponseWrapper
    {
        public string Token { get; }


        public LoginResponseWrapper(string token) =>
            Token = token;
    }
}
