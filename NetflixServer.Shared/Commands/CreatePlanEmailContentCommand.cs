namespace NetflixServer.Shared.Commands
{
    public class CreatePlanEmailContentCommand
    {
        public NotificationType NotificationType { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PlanName { get; set; }

        public decimal PlanPrice { get; set; }
    }
}
