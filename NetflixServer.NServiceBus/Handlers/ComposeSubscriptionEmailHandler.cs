using NetflixServer.NServiceBus.Services;
using NetflixServer.Shared.Commands;
using NetflixServer.Shared.Commands.Responses;
using NServiceBus;
using System.Threading.Tasks;

namespace NetflixServer.NServiceBus.Handlers
{
    public class ComposeSubscriptionEmailHandler : IHandleMessages<CreateSubscriptionEmailContentCommand>
    {
        private readonly NotificationContentService _notificationContentService;

        public ComposeSubscriptionEmailHandler(NotificationContentService notificationContentService)
        {
            _notificationContentService = notificationContentService;
        }

        public async Task Handle(CreateSubscriptionEmailContentCommand message, IMessageHandlerContext context)
        {
            (string subject, string body) = _notificationContentService.CreateSubscriptionEmailContent(message.NotificationType, message.UserName, message.PlanName, message.PlanPrice, message.ExpirationDate);

            var sendEmailResponse = new CreateSubscriptionEmailContentResponse
            {
                Subject = subject,
                Body = body,
                Email = message.Email,
            };

            await context.Reply(sendEmailResponse);
        }
    }
}
