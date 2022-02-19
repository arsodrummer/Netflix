namespace NetflixServer.Shared
{
    public enum NotificationType
    {
        SubscriberCreated, // The new subscriber has been created
        SubscriberActivated, // The new subscription plan has been activated
        SubscriberDeactivated,
        SubscriptionPlanUpdated // The new subscription plan has been updated
    }
}
