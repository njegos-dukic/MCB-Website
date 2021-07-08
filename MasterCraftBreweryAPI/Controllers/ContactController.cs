using AutoMapper;
using Core.DTO;
using Core.ErrorHandling;
using Core.Managers;
using MasterCraftBreweryAPI.ErrorHandling;
using MasterCraftBreweryAPI.Wrapper.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Controllers
{
    [Route("contact")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.Authentication.ApiKeyScheme)]
    public class ContactController : MasterCraftBreweryControllerBase
    {
        private readonly IContactManager contactManager;

        public ContactController(IContactManager contactManager, IApiKeyManager apiKeyManager, IMapper mapper) : base(apiKeyManager, mapper)
        {
            this.contactManager = contactManager;
        }

        /// <summary>
        /// Sends contact message.
        /// </summary>
        /// <param name="wrapper">Contact message and sender's information</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Send([FromBody] ContactMessagePostWrapper wrapper)
        {
            ResultMessage<bool> isSent = await contactManager.Send(mapper.Map<ContactDTO>(wrapper));
            return HttpResultMessage.FilteredResult<ContactMessagePostResponseWrapper, bool>(isSent);
        }
    }
}
