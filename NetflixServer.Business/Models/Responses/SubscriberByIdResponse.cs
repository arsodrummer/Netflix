namespace NetflixServer.Business.Models.Responses
{
    public class SubscriberByIdResponse
    {
        public long SubscriberId { get; set; }

        public long? SubscriptionPlanId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
