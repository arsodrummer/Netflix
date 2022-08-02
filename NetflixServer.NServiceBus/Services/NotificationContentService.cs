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
                case NotificationType.UserCreated:
                    return ("Wellcome to Netflix", $"Hello {userName}! You've recently created a Netflix account. See the link below to choose your subscription plan.");
                case NotificationType.UserActivated:
                    return ("Subscription plan activation", $"Dear {userName}! You've recently chosen '{subscriptionPlanName}' subscription plan. Activated until {expDate}");
                case NotificationType.UserDeactivated:
                    return ("User deactivated", $"Hi {userName}! Your acoount has been recently deactivated.");
                case NotificationType.SubscriptionPlanUpdated:
                    return ("Subscription plan update", $"Dear {userName}! Your subscription plan has been updated. Price: {price}, Expiration date: {expDate}");
                case NotificationType.SubscriptionPlanExpired:
                    return ("Subscription plan expirated", $"Dear {userName}! Your subscription plan will be expired on {expDate}. Choose another one or prolonged this one.");
                default:
                    return (string.Empty, string.Empty);
            }
        }
    }
}
