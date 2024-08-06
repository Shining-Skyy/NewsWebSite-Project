using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Email
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task Execute(string UserEmail, string Body, string Subject)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 1000000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_configuration["EmailService:Email"], _configuration["EmailService:Password"]);

                MailMessage message = new MailMessage(_configuration["EmailService:Email"], UserEmail, Subject, Body);
                message.IsBodyHtml = true;
                message.BodyEncoding = UTF8Encoding.UTF8;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                client.Send(message);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw (new Exception("Mail send failed to loginId " + UserEmail + ", though registration done." + ex.ToString() + "\n" + ex.StackTrace));
            }
        }
    }
}
