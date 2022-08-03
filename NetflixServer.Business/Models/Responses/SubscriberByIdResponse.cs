namespace NetflixServer.Business.Models.Responses
{
    public class UserByIdResponse
    {
        public long UserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public bool Active { get; set; }
    }
}
