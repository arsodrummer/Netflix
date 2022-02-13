namespace NetflixServer.Models.Requests
{
    public class NewSubscriptionPlanRequest
    {
        public decimal Price { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
