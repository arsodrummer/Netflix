namespace NetflixServer.Shared.Commands
{
    public class SubscriptionNotificationCommand
    {
        public long Id { get; set; }

        public string PlanName { get; set; }

        public decimal PlanPrice { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}
