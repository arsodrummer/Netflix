using System;

namespace NetflixServer.Api.Models.Requests
{
    public class NewSubscriptionRequest
    {
        public long UserId { get; set; }

        public long PlanId { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
