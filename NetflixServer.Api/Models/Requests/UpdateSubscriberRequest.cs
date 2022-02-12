namespace NetflixServer.Models.Requests
{
    public class UpdateSubscriberRequest
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
