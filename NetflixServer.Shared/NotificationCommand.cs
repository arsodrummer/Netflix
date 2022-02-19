using System;

namespace NetflixServer.Shared
{
    public class NotificationCommand
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public bool Active { get; set; }

        public decimal SubscriptionPlanPrice { get; set; }

        public string SubscriptionPlanName { get; set; }

        public string SubscriptionPlanDescription { get; set; }

        public DateTime? SubscriptionPlanExpirationDate { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}
