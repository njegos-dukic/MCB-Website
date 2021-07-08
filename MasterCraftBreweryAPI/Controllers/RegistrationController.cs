using AutoMapper;
using Core.DTO;
using Core.ErrorHandling;
using Core.Managers;
using MasterCraftBreweryAPI.ErrorHandling;
using MasterCraftBreweryAPI.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Controllers
{
    [Route("registration")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.Authentication.ApiKeyScheme)]
    public class RegistrationController : MasterCraftBreweryControllerBase
    {
        private readonly IAdministratorManager administratorManager;

        public RegistrationController(IAdministratorManager administratorManager, IApiKeyManager apiKeyManager, IMapper mapper) : base(apiKeyManager, mapper)
            => (this.administratorManager) = (administratorManager);

        /// <summary>
        /// Authorized administrator can register another administrator by calling this endpoint.
        /// </summary>
        /// <param name="wrapper">Credentials of the new administrator</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> RegisterAsAdministrator([FromBody] AdministratorPostWrapper wrapper)
        {
            AdministratorDTO dto = mapper.Map<AdministratorDTO>(wrapper);
            ResultMessage<bool> isRegistered = await administratorManager.Add(dto, await apiKeyManager.GetRelatedCompanyId());
            return HttpResultMessage.FilteredResult<AdministratorPostResponseWrapper, bool>(isRegistered);
        }
    }
}
