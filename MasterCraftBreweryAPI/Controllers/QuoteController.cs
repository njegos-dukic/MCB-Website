using AutoMapper;
using Core.DTO;
using Core.ErrorHandling;
using Core.Managers;
using MasterCraftBreweryAPI.ErrorHandling;
using MasterCraftBreweryAPI.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Controllers
{
    [Route("quotes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.Authentication.ApiKeyScheme)]
    public class QuoteController : MasterCraftBreweryControllerBase
    {
        private readonly IQuoteManager quoteManager;

        public QuoteController(IQuoteManager quoteManager, IApiKeyManager apiKeyManager, IMapper mapper) : base(apiKeyManager, mapper)
            => (this.quoteManager) = (quoteManager);

        /// <summary>
        /// Adds new quote to the database.
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Add([FromBody] QuotePostWrapper quote)
        {
            ResultMessage<QuoteDTO> result = await quoteManager.Add(mapper.Map<QuoteDTO>(quote));
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Updates quote with data that is sent from body.
        /// Quote must be identified using its unique identifier.
        /// If there is no such quote in the database, OperationStatus.NotFound is returned.
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Update([FromBody] QuotePutWrapper quote)
        {
            ResultMessage<QuoteDTO> result = await quoteManager.Update(mapper.Map<QuoteDTO>(quote));
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Completely deletes a quote from the database, based on specific unique identifier.
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        [HttpDelete("{quoteId}")]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Delete([FromRoute] int quoteId)
        {
            ResultMessage<bool> result = await quoteManager.Delete(quoteId);
            return HttpResultMessage.FilteredResult<QuoteDeleteWrapper, bool>(result);
        }

        /// <summary>
        /// Returns all current quotes from database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            ResultMessage<IAsyncEnumerable<QuoteDTO>> result = await quoteManager.GetAll();
            return HttpResultMessage.FilteredResult(result);
        }
    }
}
