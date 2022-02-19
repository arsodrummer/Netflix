using System;

namespace NetflixServer.Models.Requests
{
    public class UpdateSubscriberRequest
    {
        public long SubscriberId { get; set; }

        public long SubscriptionPlanId { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
