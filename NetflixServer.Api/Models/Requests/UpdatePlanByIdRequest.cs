namespace NetflixServer.Api.Models.Requests
{
    public class UpdatePlanByIdRequest
    {
        public int UserId { get; set; }

        public decimal Price { get; set; }
    }
}
