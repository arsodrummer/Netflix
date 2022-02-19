using PetaPoco;

namespace NetflixServer.Resources.Entities
{
    [TableName("SUBSCRIBER")]
    public class SubscriberEntity
    {
        [Column(Name = "ID_SUBSCRIBER")]
        public long SubscriberId { get; set; }

        [Column(Name = "ID_SUBSCRIPTION_PLAN")]
        public long? SubscriptionPlanId { get; set; }

        [Column(Name = "EMAIL")]
        public string Email { get; set; }

        [Column(Name = "USER_NAME")]
        public string UserName { get; set; }

        [Column(Name = "ACTIVE")]
        public bool Active { get; set; }
    }
}
