using NetflixServer.Business.Interfaces;
using NetflixServer.Business.Services;
using NetflixServer.NServiceBus.Sagas.SagaDatas;
using NetflixServer.NServiceBus.Services;
using NetflixServer.NServiceBus.Timeouts;
using NetflixServer.Shared;
using NetflixServer.Shared.Commands;
using NetflixServer.Shared.Email;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace NetflixServer.NServiceBus.Sagas
{
    internal class NotificationSaga : Saga<NotificationSagaData>,
                    IAmStartedByMessages<UserNotificationCommand>,
                    IAmStartedByMessages<PlanNotificationCommand>,
                    IAmStartedByMessages<SubscriptionNotificationCommand>,
                    IHandleMessages<SendEmailResponse>,
                    IHandleTimeouts<SubscriptionTimeout>
    {
        private readonly NotificationContentService _notificationContentService;
        private readonly ISubscriptionService _subscriptionService;

        public NotificationSaga(NotificationContentService notificationContentService, ISubscriptionService subscriptionService)
        {
            _notificationContentService = notificationContentService;
            _subscriptionService = subscriptionService;
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
            Data.FinishSaga = true;
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
            Data.FinishSaga = true;
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
            Data.FinishSaga = true;
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
                Data.FinishSaga = false;
                var timeoutMessage = new SubscriptionTimeout
                {
                    SubscriptionId = message.Id,
                    ExpirationDate = message.ExpirationDate,
                    UserEmail = message.UserEmail,
                    UserName = message.UserName,
                };

                await RequestTimeout(context, DateTime.SpecifyKind(/*message.ExpirationDate.AddDays(-10)*/DateTime.Now.AddSeconds(20), DateTimeKind.Local), timeoutMessage);
                await RequestTimeout(context, DateTime.SpecifyKind(/*message.ExpirationDate.AddDays(-3)*/DateTime.Now.AddSeconds(40), DateTimeKind.Local), timeoutMessage);
                timeoutMessage.IsLastNotification = true;
                await RequestTimeout(context, DateTime.SpecifyKind(/*message.ExpirationDate.AddDays(-1)*/DateTime.Now.AddSeconds(60), DateTimeKind.Local), timeoutMessage);
            }
        }

        public async Task Timeout(SubscriptionTimeout state, IMessageHandlerContext context)
        {
            (string subject, string body) = _notificationContentService.CreateSubscriptionExpiredEmailContent(state.UserName, state.ExpirationDate);

            var sendEmailCommand = new SendEmailCommand
            {
                EmailAddress = state.UserEmail,
                Body = body,
                Subject = subject,
            };

            await context.SendLocal(sendEmailCommand);

            if (state.IsLastNotification)
            {
                await _subscriptionService.DeleteSubscriptionByIdAsync(state.SubscriptionId, new System.Threading.CancellationToken());
                MarkAsComplete();
            }
        }

        public Task Handle(SendEmailResponse message, IMessageHandlerContext context)
        {
            if (Data.FinishSaga)
                MarkAsComplete();
            
            return Task.CompletedTask;
        }
    }
}
