namespace NetflixServer.Shared.Email
{
    public class SendEmailCommand
    {
        public string EmailAddress { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }
    }
}
