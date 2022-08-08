using System;

namespace NetflixServer.Business.Models.Responses
{
    public class SubscriptionByIdResponse
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long PlanId { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
