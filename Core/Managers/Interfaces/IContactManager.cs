using Core.DTO;
using Core.ErrorHandling;
using System.Threading.Tasks;

namespace Core.Managers
{
    public interface IContactManager
    {
        Task<ResultMessage<bool>> Send(ContactDTO contactInfo);
    }
}
