using MailKit.Security;
using MailKit;

using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Monshop.BackEnd.Service.Contracts;

namespace Monshop.BackEnd.Service.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string recipient, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("QK Back End Project", _configuration["Email:User"]));
            message.To.Add(new MailboxAddress("Customer", recipient));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();

            using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate(_configuration["Email:User"], _configuration["Email:ApplicationPassword"]);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
