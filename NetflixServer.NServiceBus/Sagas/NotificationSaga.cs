﻿using NetflixServer.NServiceBus.Sagas.SagaDatas;
using NetflixServer.NServiceBus.Services;
using NetflixServer.Shared.Email;
using NetflixServer.Shared;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace NetflixServer.NServiceBus.Sagas
{
    internal class NotificationSaga : Saga<NotificationSagaData>,
                    IAmStartedByMessages<NotificationCommand>,
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
            mapper.ConfigureMapping<NotificationCommand>(message => $"{message.Id}")
                    .ToSaga(saga => saga.CorrelationId);
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

            if (message.SubscriptionPlanExpirationDate - DateTime.UtcNow > TimeSpan.Zero)
            {
                await RequestTimeout(context, TimeSpan.FromSeconds(2), message);
            }
            else
            {
                await context.SendLocal(sendEmailCommand);
            }
        }

        public Task Handle(SendEmailResponse message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            return Task.CompletedTask;
        }

        public Task Timeout(NotificationCommand state, IMessageHandlerContext context)
        {
            // ...
            // construct email and send it
            // ...
            
            MarkAsComplete();
            return Task.CompletedTask;
        }
    }
}
