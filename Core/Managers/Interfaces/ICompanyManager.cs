using Core.DTO;
using Core.ErrorHandling;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Managers
{
    public interface ICompanyManager
    {
        Task<ResultMessage<DetailedCompanyDTO>> Update(DetailedCompanyDTO detailedCompanyDTO);

        Task<ResultMessage<DetailedCompanyDTO>> GetById(int uniqueIdentifier);
    }
}
