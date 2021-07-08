using Core.DTO;
using Core.ErrorHandling;
using System.Threading.Tasks;

namespace Core.Managers
{
    public interface IAdministratorManager
    {
        Task<ResultMessage<bool>> Add(AdministratorDTO administrator, int companyId);
    }
}
