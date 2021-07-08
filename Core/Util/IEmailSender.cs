using Core.DTO;
using System.Threading.Tasks;

namespace Core.Util
{
    public interface IEmailSender
    {
        Task EmailOrder(OutputOrderDTO order);

        Task SendContactMessage(ContactDTO contactInfo);
    }
}
