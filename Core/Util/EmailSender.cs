using Core.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Core.Util
{
    public class EmailSender : IEmailSender
    {
        private readonly MailConfiguration mailConfiguration;
        public readonly string sendOrderNotificationMessageFormat = "{0}  {1}";
        public readonly string sendOrderNotificationMessageSubject = "Narudžba";
        public readonly string sendOrderNotificationMessage = "Stigla je narudžba.";
        public readonly string sendContactMessageSubjectFormat = "Poruka od {0}";

        public readonly Dictionary<string, string> AttachmentsAndHTMLFormats = new Dictionary<string, string>()
        {
            {"MCBLogo","MCBLogo.png" },
            {"ContactMessageEmailFormat","emailTemplate.html" },
            {"OrderEmailFormat","orderEmailTemplate.html" },
            {"ContactEmailFormat","contactEmailTemplate.html" },
            {"OrderTableFormat", "tableStyle.json" }
        };

        /// <summary>
        /// Constructs the object reading mail configuration specified in .json file.
        /// </summary>
        public EmailSender()
        {
            mailConfiguration = JsonConvert.DeserializeObject<MailConfiguration>(StreamUtil.GetManifestResourceString("mailConfiguration.json"));
        }

        /// <summary>
        /// Sends an email to the given address, containing order details.
        /// </summary>
        /// <param name="order">Information about the order</param>
        /// <returns></returns>
        public async Task EmailOrder(OutputOrderDTO order)
        {
            await SendOrderEmail(sendOrderNotificationMessageSubject, order, AttachmentsAndHTMLFormats["OrderEmailFormat"], null, mailConfiguration.Receipient);
        }

        /// <summary>
        /// Sends an email to the given address, containing contact message.
        /// </summary>
        /// <param name="contactInfo">Contact message and sender's information</param>
        /// <returns></returns>
        public async Task SendContactMessage(ContactDTO contactInfo)
        {
            string formattedSubject = string.Format(sendContactMessageSubjectFormat, contactInfo.FirstName);
            await SendContactEmail(formattedSubject, contactInfo, AttachmentsAndHTMLFormats["ContactEmailFormat"], null, mailConfiguration.Receipient);
        }

        /// <summary>
        /// Sends an email, in specified html format.
        /// </summary>
        /// <param name="subject">Subject</param>
        /// <param name="orderInfo">Information about the order</param>
        /// <param name="htmlFormat">Name of the html embedded email template file</param>
        /// <returns></returns>
        public async Task SendOrderEmail(string subject, OutputOrderDTO order, string htmlFormat, BasicFileInfo pdfAttachment, params string[] emails)
        {
            string body = HtmlGenerator.CreateOrderEmailBody(order, htmlFormat, AttachmentsAndHTMLFormats["OrderTableFormat"]);
            await SendEmail(subject, body, pdfAttachment, emails);
        }

        /// <summary>
        /// Sends an email, in specified html format.
        /// </summary>
        /// <param name="subject">Subject</param>
        /// <param name="contactInfo">Contact info</param>
        /// <param name="htmlFormat">Name of the html embedded email template file</param>
        /// <returns></returns>
        public async Task SendContactEmail(string subject, ContactDTO contactInfo, string htmlFormat, BasicFileInfo pdfAttachment, params string[] emails)
        {
            string body = HtmlGenerator.CreateContactEmailBody(contactInfo, htmlFormat);
            await SendEmail(subject, body, pdfAttachment, emails);
        }

        /// <summary>
        /// Sends an email, in specified html format.
        /// </summary>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <returns></returns>
        public async Task SendEmail(string subject, string body, BasicFileInfo pdfAttachment, params string[] emails)
        {
            var mailMessage = new MailMessage { From = new MailAddress(mailConfiguration.Sender) };

            using SmtpClient client = GetSmtpClient();
            foreach (string toAddress in emails)
                mailMessage.To.Add(toAddress);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;

            // If there are images needed to be displayed in html, they are embedded in email by using LinkedResource and AlternateView
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            using Stream stream = StreamUtil.GetManifestResourceStream(AttachmentsAndHTMLFormats["MCBLogo"]);
            LinkedResource mcbLogo = new LinkedResource(stream, "image/png") { ContentId = "mcbLogo" };
            alternateView.LinkedResources.Add(mcbLogo);
            mailMessage.AlternateViews.Add(alternateView);
            if (pdfAttachment != null)
            {
                Attachment attachment = new Attachment(new MemoryStream(pdfAttachment.FileData), pdfAttachment.FileName, MediaTypeNames.Application.Pdf);
                mailMessage.Attachments.Add(attachment);
            }
            await client.SendMailAsync(mailMessage);
        }
        /// <summary>
        /// Returns SMPT client with credentials
        /// </summary>
        /// <returns></returns>
        private SmtpClient GetSmtpClient()
           => new SmtpClient
           {
               EnableSsl = false,
               Port = mailConfiguration.Port,
               Host = mailConfiguration.Host,
               UseDefaultCredentials = true,
               Credentials = new NetworkCredential(mailConfiguration.Username, mailConfiguration.Password),
               Timeout = 10000
           };
    }
}
