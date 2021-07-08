using Core.DTO;
using Core.ErrorHandling;
using Core.Util;
using System;
using System.Threading.Tasks;

namespace Core.Managers
{
    public class ContactManager : IContactManager
    {
        private readonly IEmailSender emailSender;

        public ContactManager(IEmailSender emailSender)
             => this.emailSender = emailSender;

        /// <summary>
        /// Sends contact message.
        /// </summary>
        /// <param name="contactInfo">Contact message and sender's information</param>
        /// <returns></returns>
        public async Task<ResultMessage<bool>> Send(ContactDTO contactInfo)
        {
            try
            {
                if (!InputValidator.IsValidEmail(contactInfo.Email))
                    return new ResultMessage<bool>(OperationStatus.InvalidData);

                await emailSender.SendContactMessage(contactInfo);
                return new ResultMessage<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResultMessage<bool>(OperationStatus.UnknownError, ex.Message);
            }
        }
    }
}
