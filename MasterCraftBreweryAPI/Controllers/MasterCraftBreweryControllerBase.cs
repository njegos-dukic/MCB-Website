using AutoMapper;
using Core.Managers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MasterCraftBreweryAPI.Controllers
{
    [EnableCors("Wildcard")]
    public class MasterCraftBreweryControllerBase : ControllerBase
    {
        protected readonly IApiKeyManager apiKeyManager;
        protected readonly IMapper mapper;

        public MasterCraftBreweryControllerBase(IApiKeyManager apiKeyManager, IMapper mapper)
            => (this.apiKeyManager, this.mapper) = (apiKeyManager, mapper);
    }
}
