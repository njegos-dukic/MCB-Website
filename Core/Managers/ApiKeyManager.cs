using Core.Entity;
using Core.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Managers
{
    public class ApiKeyManager : IApiKeyManager
    {
        private readonly string apiKey;
        private readonly MasterCraftBreweryContext context;

        public ApiKeyManager(string apiKey, MasterCraftBreweryContext context)
        {
            this.apiKey = apiKey;
            this.context = context;
        }

        public async Task<bool> Exists(string key)
            => await context.Companies.Select(x => x.ApiKey).AnyAsync(x => x == key);

        public string GetApiKey()
             => apiKey;

        public async Task<bool> IsRelatedToCompany(int companyId)
            => await GetRelatedCompanyId() == companyId;

        /// <summary>
        /// Based on api key, returns unique identificator for the company.
        /// </summary>
        /// <exception cref="ApiKeyAuthenticationException"></exception>
        public async Task<int> GetRelatedCompanyId()
        {
            int? companyId = (await context.Companies
                                       .Select(x => new { x.ApiKey, x.CompanyId })
                                       .SingleOrDefaultAsync(x => x.ApiKey == apiKey))
                                       ?.CompanyId;
            if (companyId == null)
                throw new ApiKeyAuthenticationException();
            return companyId.Value;
        }
    }
}
