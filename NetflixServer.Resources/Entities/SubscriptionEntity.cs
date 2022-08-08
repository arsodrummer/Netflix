using PetaPoco;
using System;

namespace NetflixServer.Resources.Entities
{
    [TableName("SUBSCRIPTIONS")]
    public class SubscriptionEntity
    {
        [Column(Name = "ID")]
        public long SubscriptionId { get; set; }

        [Column(Name = "ID_USER")]
        public long UserId { get; set; }

        [Column(Name = "ID_PLAN")]
        public long PlanId { get; set; }

        [Column(Name = "EXPIRATION_DATE")]
        public DateTime ExpirationDate { get; set; }
    }
}
