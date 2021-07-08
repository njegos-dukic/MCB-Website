using Core.AutoMapper.ExtensionMethods;
using Core.DTO;
using Core.Entity;
using Core.ErrorHandling;
using Core.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Managers
{
    public class CompanyManager : ICompanyManager
    {
        private readonly IApiKeyManager apiKeyManager;
        private readonly MasterCraftBreweryContext context;

        public CompanyManager(IApiKeyManager apiKeyManager, MasterCraftBreweryContext context)
            => (this.apiKeyManager, this.context) = (apiKeyManager, context);

        /// <summary>
        /// Adds the company with basic information such as name and address, and with 
        /// list of social medias and wholesales related to that company. 
        /// These lists can be empty or null.
        /// </summary>
        /// <param name="detailedCompanyDTO">The object that contains data to be added</param>
        public async Task<ResultMessage<DetailedCompanyDTO>> Add(DetailedCompanyDTO detailedCompanyDTO)
        {
            try
            {
                if (!IsValidCompany(detailedCompanyDTO))
                    return new ResultMessage<DetailedCompanyDTO>(OperationStatus.InvalidData);

                if (await context.Companies.AnyAsync(x => x.Name == detailedCompanyDTO.Name))
                    return new ResultMessage<DetailedCompanyDTO>(OperationStatus.Exists);

                Company company = detailedCompanyDTO.ToEntity();
                company.ApiKey = Guid.NewGuid().ToString();
                await context.Companies.AddAsync(company);
                await context.SaveChangesAsync();

                return new ResultMessage<DetailedCompanyDTO>(company.ToDetailedDto());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<DetailedCompanyDTO>(status, message);
            }
        }

        /// <summary>
        /// Updates company based on a unique identifier assuming that whole Company object is going to be updated.
        /// </summary>
        /// <param name="detailedCompanyDTO">The object containing new information</param>
        /// <returns></returns>
        public async Task<ResultMessage<DetailedCompanyDTO>> Update(DetailedCompanyDTO detailedCompanyDTO)
        {
            try
            {
                if (!await context.Companies.AnyAsync(x => x.CompanyId == detailedCompanyDTO.CompanyId))
                    return new ResultMessage<DetailedCompanyDTO>(OperationStatus.NotFound);

                if (!IsValidCompany(detailedCompanyDTO))
                    return new ResultMessage<DetailedCompanyDTO>(OperationStatus.InvalidData);

                Company newCompany = detailedCompanyDTO.ToEntity();

                if (await NotAuthenticated(detailedCompanyDTO.CompanyId))
                    throw new ForbiddenAccessException();

                Company company = await context.Companies.Include(x => x.Wholesales)
                                                         .Include(x => x.SocialMedias)
                                                         .Include(x => x.Phones)
                                                         .SingleOrDefaultAsync(x => x.CompanyId == detailedCompanyDTO.CompanyId);

                company.SocialMedias = newCompany.SocialMedias;
                company.Wholesales = newCompany.Wholesales;
                company.Phones = newCompany.Phones;
                newCompany.ApiKey = company.ApiKey;
                context.Entry(company).CurrentValues.SetValues(newCompany);
                await context.SaveChangesAsync();

                return new ResultMessage<DetailedCompanyDTO>(company.ToDetailedDto());
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string message) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<DetailedCompanyDTO>(status, message);
            }
        }

        /// <summary>
        /// Returns a company with details such as social media and wholesales, based on 
        /// the specified unique identifier for the company.
        /// </summary>
        /// <param name="uniqueIdentifier">Unique identifier for the company</param>
        /// <returns>Company with details, if such is found, or null otherwise</returns>
        public async Task<ResultMessage<DetailedCompanyDTO>> GetById(int uniqueIdentifier)
        {
            Company company = await GetByFilter(x => x.CompanyId == uniqueIdentifier);

            if (company == null)
                return new ResultMessage<DetailedCompanyDTO>(OperationStatus.NotFound);

            if (await NotAuthenticated(uniqueIdentifier))
                throw new ForbiddenAccessException();

            return new ResultMessage<DetailedCompanyDTO>(company.ToDetailedDto());
        }

        private bool IsValidCompany(DetailedCompanyDTO dto)
           => InputValidator.IsValidEmail(dto.Email)
              && InputValidator.IsValidPostalCode(dto.PostalCode)
              && !dto.SocialMedias.Any(x => !InputValidator.IsValidHttpUrl(x.Url))
              && !dto.Wholesales.Any(x => !InputValidator.AreValidCoordinates(x.Coordinates))
              && !dto.Phones.Any(x => !InputValidator.IsValidPhoneNumber(x.PhoneNumber));

        private async Task<Company> GetByFilter(Expression<Func<Company, bool>> condition) =>
            await context.Companies.Include(x => x.Wholesales)
                                   .Include(x => x.SocialMedias)
                                   .Include(x => x.Phones)
                                   .SingleOrDefaultAsync(condition);

        private async Task<bool> NotAuthenticated(int companyId)
            => !await apiKeyManager.IsRelatedToCompany(companyId);
    }
}
