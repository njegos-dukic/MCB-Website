using AutoMapper;
using Core.ErrorHandling;
using Core.Managers;
using MasterCraftBreweryAPI.ErrorHandling;
using MasterCraftBreweryAPI.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Controllers
{
    [Route("login")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.Authentication.ApiKeyScheme)]
    public class LoginController : MasterCraftBreweryControllerBase
    {
        private readonly ILoginManager loginManager;

        public LoginController(ILoginManager loginManager, IApiKeyManager apiKeyManager, IMapper mapper) : base(apiKeyManager, mapper)
        {
            this.loginManager = loginManager;
        }

        /// <summary>
        /// Login to the application using credentials, and get authentication token as base64 string.
        /// </summary>
        /// <param name="data">Credentials</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginPostWrapper data)
        {
            ResultMessage<string> token = await loginManager.Login(data.Email, data.Password);
            return HttpResultMessage.FilteredResult<LoginResponseWrapper, string>(token);
        }

        /// <summary>
        /// Checks if user is logged in.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "TokenRequired")]
        public ActionResult IsLoggedIn()
            => HttpResultMessage.FilteredResult<IsLoggedInResponseWrapper, bool>(new ResultMessage<bool>(true));
    }
}
