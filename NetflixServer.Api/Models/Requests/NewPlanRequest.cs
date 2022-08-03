using System;

namespace NetflixServer.Models.Requests
{
    public class NewPlanRequest
    {
        public decimal Price { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
