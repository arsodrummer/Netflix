using System;

namespace NetflixServer.Api.Models.Requests
{
    public class UpdatePlanByIdRequest
    {
        public DateTime? ExpirationDate { get; set; }
    }
}
