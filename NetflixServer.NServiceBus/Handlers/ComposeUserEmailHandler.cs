using NetflixServer.NServiceBus.Services;
using NetflixServer.Shared.Commands;
using NetflixServer.Shared.Commands.Responses;
using NServiceBus;
using System.Threading.Tasks;

namespace NetflixServer.NServiceBus.Handlers
{
    public class ComposeUserEmailHandler : IHandleMessages<CreateUserEmailContentCommand>
    {
        private readonly NotificationContentService _notificationContentService;

        public ComposeUserEmailHandler(NotificationContentService notificationContentService)
        {
            _notificationContentService = notificationContentService;
        }

        public async Task Handle(CreateUserEmailContentCommand message, IMessageHandlerContext context)
        {
            (string subject, string body) = _notificationContentService.CreateUserEmailContent(message.NotificationType, message.UserName);

            var sendEmailResponse = new CreateUserEmailContentResponse
            {
                Subject = subject,
                Body = body,
                Email = message.Email,
            };

            await context.Reply(sendEmailResponse);
        }
    }
}
