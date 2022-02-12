namespace NetflixServer.Models.Requests
{
    public class NewSubscriberRequest
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
