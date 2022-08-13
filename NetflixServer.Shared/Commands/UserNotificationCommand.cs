namespace NetflixServer.Shared.Commands
{
    public class UserNotificationCommand
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}
