using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Mtama.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {

            //Email
            var fromAddress = new MailAddress(ConfigurationManager.GetAppSetting("EmailService"), ConfigurationManager.GetAppSetting("ForgotYourPasswordString"));
            var toAddress = new MailAddress(email);
            var fromPassword = ConfigurationManager.GetAppSetting("EmailServicePassword");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 15000
            };
            using (var message1 = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml=true
            })
            {
                smtp.Send(message1);
            }
            return Task.CompletedTask;
        }
    }
}
