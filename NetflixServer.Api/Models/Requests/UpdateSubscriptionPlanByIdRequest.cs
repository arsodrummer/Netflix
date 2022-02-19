using System;

namespace NetflixServer.Api.Models.Requests
{
    public class UpdateSubscriptionPlanByIdRequest
    {
        public decimal Price { get; set; }

        public string Name { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
