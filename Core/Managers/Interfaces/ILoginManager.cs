using Core.ErrorHandling;
using System.Threading.Tasks;

namespace Core.Managers
{
    public interface ILoginManager
    {

        Task<ResultMessage<string>> Login(string email, string password);

        Task<ResultMessage<bool>> IsValidToken(string token);
    }
}
