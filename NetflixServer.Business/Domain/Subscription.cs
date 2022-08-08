using System;

namespace NetflixServer.Business.Domain
{
    public class Subscription
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long PlanId { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
