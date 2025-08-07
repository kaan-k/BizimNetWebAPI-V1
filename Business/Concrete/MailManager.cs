using Business.Abstract;
using Entities.Concrete.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MailManager : IMailManager
    {
        public void SendInstallationConfirmationMail(EmailConfiguration config, EMailContent content)
        {
            using var client = new SmtpClient(config.SmtpServer)
            {
                Port = config.Port,
                Credentials = new NetworkCredential(config.Username, config.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(config.From),
                Subject = content.Subject,
                Body = content.Body,
                IsBodyHtml = content.IsBodyHtml
            };
        }

        public void SendMail(EmailConfiguration config, EMailContent content)
        {
            using var client = new SmtpClient(config.SmtpServer)
            {
                Port = config.Port,
                Credentials = new NetworkCredential(config.Username, config.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(config.From),
                Subject = content.Subject,
                Body = content.Body,
                IsBodyHtml = content.IsBodyHtml
            };

            foreach (var recipient in config.To)
            {

                mailMessage.To.Add(recipient);
            }

            //client.Send(mailMessage);
        }
    }

}
