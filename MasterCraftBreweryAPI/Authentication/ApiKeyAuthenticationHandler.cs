using Core.ErrorHandling;
using Core.Managers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IApiKeyManager apiKeyManager;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IApiKeyManager apiKeyManager) : base(options, logger, encoder, clock)
        {
            this.apiKeyManager = apiKeyManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("X-API-KEY"))
                return AuthenticateResult.Fail(new ApiKeyAuthenticationException());

            string apiKey = Request.Headers["X-API-KEY"];

            if (!await apiKeyManager.Exists(apiKey))
                return AuthenticateResult.Fail(new ApiKeyAuthenticationException());

            Claim[] claims = new[]
             {
                 new Claim(ClaimTypes.Anonymous, apiKey)
             };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, Authentication.ApiKeyScheme);
            AuthenticationTicket ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties(), Authentication.ApiKeyScheme);
            return AuthenticateResult.Success(ticket);
        }
    }
}
