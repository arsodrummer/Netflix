using NetflixServer.NServiceBus.Services;
using NetflixServer.Shared.Commands;
using NetflixServer.Shared.Commands.Responses;
using NServiceBus;
using System.Threading.Tasks;

namespace NetflixServer.NServiceBus.Handlers
{
    public class ComposePlanEmailHandler : IHandleMessages<CreatePlanEmailContentCommand>
    {
        private readonly NotificationContentService _notificationContentService;

        public ComposePlanEmailHandler(NotificationContentService notificationContentService)
        {
            _notificationContentService = notificationContentService;
        }

        public async Task Handle(CreatePlanEmailContentCommand message, IMessageHandlerContext context)
        {
            (string subject, string body) = _notificationContentService.CreatePlanEmailContent(message.NotificationType, message.UserName, message.PlanName, message.PlanPrice);

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
