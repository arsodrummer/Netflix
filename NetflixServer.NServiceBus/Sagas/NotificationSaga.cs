using NetflixServer.NServiceBus.Sagas.SagaDatas;
using NetflixServer.NServiceBus.Services;
using NetflixServer.Shared.Email;
using NetflixServer.Shared;
using NServiceBus;
using System;
using System.Threading.Tasks;
using NetflixServer.Shared.Commands;

namespace NetflixServer.NServiceBus.Sagas
{
    internal class NotificationSaga : Saga<NotificationSagaData>,
                    IAmStartedByMessages<NotificationCommand>,
                    IAmStartedByMessages<UserNotificationCommand>,
                    IAmStartedByMessages<PlanNotificationCommand>,
                    IAmStartedByMessages<SubscriptionNotificationCommand>,
                    IHandleMessages<SendEmailResponse>,
                    IHandleTimeouts<NotificationCommand>
    {
        private readonly NotificationContentService _notificationContentService;

        public NotificationSaga(NotificationContentService notificationContentService)
        {
            _notificationContentService = notificationContentService;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<NotificationSagaData> mapper)
        {
            mapper.MapSaga(saga => saga.CorrelationId)
                .ToMessage<NotificationCommand>(message => $"{message.Id}")
                .ToMessage<UserNotificationCommand>(message => $"{message.Id}")
                .ToMessage<PlanNotificationCommand>(message => $"{message.Id}")
                .ToMessage<SubscriptionNotificationCommand>(message => $"{message.Id}");
        }

        public async Task Handle(NotificationCommand message, IMessageHandlerContext context)
        {
            (string subject, string body) = _notificationContentService.CreateEmailContent(message.NotificationType, message.UserName, message.SubscriptionPlanName, message.SubscriptionPlanExpirationDate, message.SubscriptionPlanPrice);

            var sendEmailCommand = new SendEmailCommand
            {
                EmailAddress = message.Email,
                Body = body,
                Subject = subject,
            };

            if (message.NotificationType == NotificationType.UserActivated)
            {
                await RequestTimeout(context, TimeSpan.FromSeconds(2), message);
            }
            else
            {
                await context.SendLocal(sendEmailCommand);
            }
        }

        public async Task Handle(UserNotificationCommand message, IMessageHandlerContext context)
        {
            (string subject, string body) = _notificationContentService.CreateUserEmailContent(message.NotificationType, message.UserName);

            var sendEmailCommand = new SendEmailCommand
            {
                EmailAddress = message.Email,
                Body = body,
                Subject = subject,
            };

            await context.SendLocal(sendEmailCommand);
        }

        public async Task Handle(PlanNotificationCommand message, IMessageHandlerContext context)
        {
            (string subject, string body) = _notificationContentService.CreatePlanEmailContent(message.NotificationType, message.UserName, message.PlanName, message.PlanPrice);

            var sendEmailCommand = new SendEmailCommand
            {
                EmailAddress = message.UserEmail,
                Body = body,
                Subject = subject,
            };

            await context.SendLocal(sendEmailCommand);
        }

        public async Task Handle(SubscriptionNotificationCommand message, IMessageHandlerContext context)
        {
            (string subject, string body) = _notificationContentService.CreateSubscriptionEmailContent(message.NotificationType, message.UserName, message.PlanName, message.PlanPrice);

            var sendEmailCommand = new SendEmailCommand
            {
                EmailAddress = message.UserEmail,
                Body = body,
                Subject = subject,
            };

            await context.SendLocal(sendEmailCommand);
        }

        public Task Handle(SendEmailResponse message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            return Task.CompletedTask;
        }

        public async Task Timeout(NotificationCommand state, IMessageHandlerContext context)
        {
            (string subject, string body) = _notificationContentService.CreateEmailContent(NotificationType.SubscriptionPlanExpired, state.UserName, state.SubscriptionPlanName, state.SubscriptionPlanExpirationDate, state.SubscriptionPlanPrice);

            var sendEmailCommand = new SendEmailCommand
            {
                EmailAddress = state.Email,
                Body = body,
                Subject = subject,
            };

            await context.SendLocal(sendEmailCommand);
        }
    }
}
