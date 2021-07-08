using AutoMapper;
using Core.DTO;
using Core.ErrorHandling;
using Core.Managers;
using MasterCraftBreweryAPI.ErrorHandling;
using MasterCraftBreweryAPI.Wrapper.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Controllers
{
    /// <summary>
    /// Contains endpoints and logic for manipulating company 
    /// information, like basic information, social media and wholesale places.
    /// </summary>
    [Route("companies")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.Authentication.ApiKeyScheme)]
    public class CompanyController : MasterCraftBreweryControllerBase
    {
        private readonly ICompanyManager companyManager;

        public CompanyController(ICompanyManager companyManager, IApiKeyManager apiKeyManager, IMapper mapper) : base(apiKeyManager, mapper)
         => (this.companyManager) = (companyManager);

        /// <summary>
        /// Updates company based on a unique identifier assuming that whole Company object is going to be updated.
        /// </summary>
        /// <param name="company">The object containing new information <see cref="DetailedCompanyPutWrapper"></see>/></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Update([FromBody] DetailedCompanyPutWrapper company)
        {
            DetailedCompanyDTO companyDTO = mapper.Map<DetailedCompanyDTO>(company);
            companyDTO.CompanyId = await apiKeyManager.GetRelatedCompanyId();

            ResultMessage<DetailedCompanyDTO> detailedCompanyResult = await companyManager.Update(companyDTO);
            return HttpResultMessage.FilteredResult(detailedCompanyResult);
        }

        ///<summary>
        /// Returns a company with details such as social media and wholesales, based on 
        /// the specified unique api key.
        /// </summary>
        /// <returns>Company with details, if such is found, or null otherwise</returns>
        [HttpGet("companyInfo")]
        public async Task<ActionResult> GetCompanyInfo()
        {
            ResultMessage<DetailedCompanyDTO> detailedCompanyResult = await companyManager.GetById(await apiKeyManager.GetRelatedCompanyId());
            return HttpResultMessage.FilteredResult(detailedCompanyResult);
        }
    }
}