using System;

namespace NetflixServer.NServiceBus.Timeouts
{
    public class SubscriptionTimeout
    {
        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsLastNotification { get; set; }
    }
}
