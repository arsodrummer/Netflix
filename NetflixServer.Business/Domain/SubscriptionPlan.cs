namespace NetflixServer.Business.Domain
{
    public class SubscriptionPlan
    {
        public long SubscriptionPlanId { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
