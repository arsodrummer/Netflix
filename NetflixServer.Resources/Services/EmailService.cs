using System.Net.Mail;

namespace NetflixServer.Resources.Services
{
    public class EmailService
    {
        public void SendEmailMessageAsync(string emailAddress, string subject, string body)
        {
            var mail = new MailMessage();
            
            MailAddress FromAddress = new MailAddress(/*ConfigurationManager.AppSettings["FromAddress"]*/"serviceBus@mail.com");

            mail.From = FromAddress;
            mail.Sender = FromAddress;
            mail.To.Add(emailAddress);

            mail.Subject = subject;
            mail.Body = body;

            var senderMail = new SmtpClient(/*ConfigurationManager.AppSettings["SMTPServerAddress"].ToString()*/"localhost");// 127.0.0.1

            senderMail.Send(mail);
        }
    }
}
