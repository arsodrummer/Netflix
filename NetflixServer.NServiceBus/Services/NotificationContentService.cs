using NetflixServer.Shared;
using System;

namespace NetflixServer.NServiceBus.Services
{
    public class NotificationContentService
    {
        public (string Subject, string Body) CreateUserEmailContent(NotificationType notificationType, string userName)
        {
            switch (notificationType)
            {
                case NotificationType.UserCreated:
                    return ("Wellcome to Netflix",
                        $"Hello {userName}! You've recently created a Netflix account. See the link below to choose your subscription plan.");
                case NotificationType.UserActivated:
                    return ("User activation",
                            $"Dear {userName}! Wellcome back to Netflix!");
                case NotificationType.UserDeactivated:
                    return ("User deactivation",
                            $"Hi {userName}! Your account has been recently deactivated. We'll miss you...");
                default:
                    return (string.Empty, string.Empty);
            }
        }

        public (string Subject, string Body) CreatePlanEmailContent(NotificationType notificationType, string userName, string planName, decimal planPrice)
        {
            switch (notificationType)
            {
                case NotificationType.PlanPriceUpdated:
                    return ("Price updated",
                            $"Hi {userName}! The price for {planName} subscription plan has been updated and now it's {planPrice} USD per month.");
                default:
                    return (string.Empty, string.Empty);
            }
        }

        public (string Subject, string Body) CreateSubscriptionEmailContent(NotificationType notificationType, string userName, string planName, decimal planPrice, DateTime expirationDate)
        {
            switch (notificationType)
            {
                case NotificationType.SubscriptionActivated:
                    return ("Subscription plan activation",
                            $"Hi {userName}! Your subscription plan {planName} with {planPrice} USD per month - has been activated until {expirationDate:dd-MM-yyyy}. Enjoy it!");
                case NotificationType.SubscriptionDeactivated:
                    return ("Subscription plan deactitarion",
                            $"Hi {userName}! Your subscription plan {planName} has been deactivated. Could you send us your decision why is it so?");
                default:
                    return (string.Empty, string.Empty);
            }
        }

        public (string Subject, string Body) CreateSubscriptionExpiredEmailContent(string userName, DateTime expirationDate) =>
            ("Subscription expiration",
            $"Hi {userName}! We'd like to notify you that your subscription will be expired on {expirationDate:dd-MM-yyyy}.");
    }
}
