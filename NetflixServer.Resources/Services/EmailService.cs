using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace NetflixServer.Resources.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmailMessageAsync(string emailAddress, string subject, string body)
        {
            var mail = new MailMessage();
            
            MailAddress FromAddress = new MailAddress(_configuration.GetSection("EmailStrings").GetSection("FromAddress").Value);
            
            mail.From = FromAddress;
            mail.Sender = FromAddress;
            mail.To.Add(emailAddress);

            mail.Subject = subject;
            mail.Body = body;

            var senderMail = new SmtpClient(_configuration.GetSection("EmailStrings").GetSection("SMTPServerAddress").Value);

            senderMail.Send(mail);
        }
    }
}
