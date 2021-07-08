using Core.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Authentication
{
    public class TokenAuthorizationHandler : AuthorizationHandler<TokenAuthorizationRequirement>
    {
        private readonly ILoginManager loginManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenAuthorizationHandler(ILoginManager loginManager, IHttpContextAccessor httpContextAccessor)
        {
            this.loginManager = loginManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenAuthorizationRequirement requirement)
        {
            HttpRequest request = httpContextAccessor.HttpContext.Request;
            if (request.Headers.ContainsKey("Authorize") && await loginManager.IsValidToken(request.Headers["Authorize"]))
                context.Succeed(requirement);
        }
    }
}
