using PetaPoco;
using System;

namespace NetflixServer.Resources.Entities
{
    [TableName("SUBSCRIPTION_PLAN")]
    public class SubscriptionPlanEntity
    {
        [Column(Name = "ID_SUBSCRIPTION_PLAN")]
        public long SubscriptionPlanId { get; set; }

        [Column(Name = "PRICE")]
        public decimal Price { get; set; }

        [Column(Name = "NAME")]
        public string Name { get; set; }

        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }

        [Column(Name = "EXPIRATION_DATE")]
        public DateTime? ExpirationDate { get; set; }
    }
}
