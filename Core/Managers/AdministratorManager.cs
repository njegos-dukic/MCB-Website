using Core.AutoMapper;
using Core.DTO;
using Core.Entity;
using Core.ErrorHandling;
using Core.Util;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Core.Managers
{
    public class AdministratorManager : IAdministratorManager
    {
        private readonly MasterCraftBreweryContext context;

        public AdministratorManager(MasterCraftBreweryContext context)
           => this.context = context;

        public async Task<ResultMessage<bool>> Add(AdministratorDTO administrator, int companyId)
        {
            try
            {
                if (await context.Administrators.AnyAsync(x => x.CompanyId == companyId && administrator.Email == x.Email))
                    return new ResultMessage<bool>(OperationStatus.Exists);

                Administrator administratorEntity = Mapping.Mapper.Map<Administrator>(administrator);
                (administratorEntity.Salt, administratorEntity.Password) = Security.ComputePassword(administrator.Password);
                administratorEntity.CompanyId = companyId;

                await context.Administrators.AddAsync(administratorEntity);
                await context.SaveChangesAsync();
                return new ResultMessage<bool>(true, OperationStatus.Success);
            }
            catch (DbUpdateException ex)
            {
                (OperationStatus status, string detailedMessage) = DbUpdateExceptionHandler.HandleException(ex);
                return new ResultMessage<bool>(status, detailedMessage);
            }
        }
    }
}
