using System;

namespace NetflixServer.Shared
{
    public class NotificationCommand
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Content { get; set; }

        public decimal SubscriptionPlanPrice { get; set; }

        public string SubscriptionPlanName { get; set; }

        public string SubscriptionPlanDescription { get; set; }
    }
}
