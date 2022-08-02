using System;

namespace NetflixServer.Models.Requests
{
    public class UpdateUserRequest
    {
        public long SubscriptionPlanId { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool Active { get; set; }
    }
}
