using System.Threading.Tasks;

namespace Core.Managers
{
    public interface IApiKeyManager
    {
        string GetApiKey();

        Task<int> GetRelatedCompanyId();

        Task<bool> Exists(string key);

        Task<bool> IsRelatedToCompany(int companyId);
    }
}
