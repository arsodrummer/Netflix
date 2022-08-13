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
                    IAmStartedByMessages<UserNotificationCommand>,
                    IAmStartedByMessages<PlanNotificationCommand>,
                    IAmStartedByMessages<SubscriptionNotificationCommand>,
                    IHandleMessages<SendEmailResponse>,
                    IHandleTimeouts<SubscriptionNotificationCommand>
    {
        private readonly NotificationContentService _notificationContentService;

        public NotificationSaga(NotificationContentService notificationContentService)
        {
            _notificationContentService = notificationContentService;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<NotificationSagaData> mapper)
        {
            mapper.MapSaga(saga => saga.CorrelationId)
                .ToMessage<UserNotificationCommand>(message => $"{message.Id}")
                .ToMessage<PlanNotificationCommand>(message => $"{message.Id}")
                .ToMessage<SubscriptionNotificationCommand>(message => $"{message.Id}");
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
            if (message.NotificationType == NotificationType.SubscriptionActivated)
                Data.SubscriptionActivated = true;

            (string subject, string body) = _notificationContentService.CreateSubscriptionEmailContent(message.NotificationType, message.UserName, message.PlanName, message.PlanPrice, message.ExpirationDate);

            var sendEmailCommand = new SendEmailCommand
            {
                EmailAddress = message.UserEmail,
                Body = body,
                Subject = subject,
            };

            await context.SendLocal(sendEmailCommand);

            if (message.NotificationType == NotificationType.SubscriptionActivated)
            {
                await RequestTimeout(context, message.ExpirationDate.AddDays(-10)/*TimeSpan.FromSeconds(2)*/, message);
                await RequestTimeout(context, message.ExpirationDate.AddDays(-3)/*TimeSpan.FromSeconds(2)*/, message);
                await RequestTimeout(context, message.ExpirationDate.AddDays(-1)/*TimeSpan.FromSeconds(2)*/, message);
            }
        }

        public async Task Timeout(SubscriptionNotificationCommand state, IMessageHandlerContext context)
        {
            //if (Data.SubscriptionActivated)
            //{
            //    return ReplyToOriginator(context, state);
            //}

            (string subject, string body) = _notificationContentService.CreateSubscriptionExpiredEmailContent(state.UserName, state.ExpirationDate);

            var sendEmailCommand = new SendEmailCommand
            {
                EmailAddress = state.UserEmail,
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
    }
}
