using NetflixServer.Shared;
using System;

namespace NetflixServer.NServiceBus.Services
{
    public class NotificationContentService
    {
        public (string Subject, string Body) CreateEmailContent(NotificationType notificationType, string userName, string subscriptionPlanName, DateTime? expDate, decimal price)
        {
            switch (notificationType)
            {
                case NotificationType.SubscriberCreated:
                    return ("Wellcome to Netflix", $"Hello {userName}! You've recently created a Netflix account. See the link below to choose your subscription plan.");
                case NotificationType.SubscriberActivated:
                    return ("Subscription plan activation", $"Dear {userName}! You've recently chosen '{subscriptionPlanName}' subscription plan. Active until {expDate}");
                case NotificationType.SubscriberDeactivated:
                    return (string.Empty, string.Empty);
                case NotificationType.SubscriptionPlanUpdated:
                    return ("Subscription plan update", $"Dear {userName}! Your subscription plan has been updated. Price: {price}, Expiration date: {expDate}");
                default:
                    return (string.Empty, string.Empty);
            }
        }
    }
}
