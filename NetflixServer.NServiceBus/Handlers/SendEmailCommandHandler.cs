using NetflixServer.Resources.Services;
using NetflixServer.Shared.Email;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace NetflixServer.NServiceBus.Handlers
{
    public class SendEmailCommandHandler : IHandleMessages<SendEmailCommand>
    {
        private readonly EmailService _emailService;

        public SendEmailCommandHandler(EmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(SendEmailCommand message, IMessageHandlerContext context)
        {
            try
            {
                _emailService.SendEmailMessageAsync(message.EmailAddress, message.Subject, message.Body);

                var sendEmailResponse = new SendEmailResponse
                {
                    SentDate = DateTime.UtcNow,
                };

                await context.Reply(sendEmailResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
