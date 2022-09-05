namespace NetflixServer.Shared.Commands
{
    public class CreateUserEmailContentCommand
    {
        public NotificationType NotificationType { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
