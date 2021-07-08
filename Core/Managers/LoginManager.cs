using Core.Entity;
using Core.ErrorHandling;
using Core.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Managers
{
    public class LoginManager : ILoginManager
    {
        private readonly IApiKeyManager apiKeyManager;
        private readonly MasterCraftBreweryContext context;

        public LoginManager(IApiKeyManager apiKeyManager, MasterCraftBreweryContext context)
        {
            this.apiKeyManager = apiKeyManager;
            this.context = context;
        }

        /// <summary>
        /// Generates security token if provided credentials are valid.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <exception cref="ForbiddenAccessException"></exception>
        /// <returns>Generated token</returns>
        public async Task<ResultMessage<string>> Login(string email, string password)
        {
            if (!await CheckCredentials(email, password))
                return new ResultMessage<string>(OperationStatus.InvalidData);

            return new ResultMessage<string>(Authentication.GenerateToken(email, password));
        }

        /// <summary>
        /// Checks if token is valid (not expired and is related to correct account).
        /// </summary>
        /// <exception cref="ForbiddenAccessException"></exception>
        /// <param name="token">Token</param>
        /// <returns></returns>
        public async Task<ResultMessage<bool>> IsValidToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return new ResultMessage<bool>(false, OperationStatus.InvalidData);
            AuthenticationData data = Authentication.ParseAuthenticationToken(token);

            if (data.ExpirationDate < DateTime.UtcNow)
                return new ResultMessage<bool>(false, OperationStatus.InvalidData);

            return new ResultMessage<bool>(await CheckCredentials(data.Email, data.Password), OperationStatus.Success);
        }

        /// <summary>
        /// Checks if account with credentials exists. If not, it returns proper error code.
        /// If account exists, but user has no access to the resource, exception is thrown. 
        /// <see cref="ForbiddenAccessException"/>. If password matches email, credentials are 
        /// valid.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        private async Task<bool> CheckCredentials(string email, string password)
        {
            // Does exist?
            Administrator administrator = await context.Administrators
                                            .Where(x => x.Email == email)
                                            .SingleOrDefaultAsync();
            if (administrator == null)
                return false;

            // Can access the resource?
            if (await NotAuthenticated(email))
                throw new ForbiddenAccessException();

            // Password matches account
            return Security.ConfirmPassword(password, administrator.Salt, administrator.Password);
        }

        private async Task<bool> NotAuthenticated(string email)
        {
            int companyId = await apiKeyManager.GetRelatedCompanyId();
            return !await context.Administrators.AnyAsync(x => x.Email == email && x.CompanyId == companyId);
        }
    }
}
