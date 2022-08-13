using NServiceBus;

namespace NetflixServer.NServiceBus.Sagas.SagaDatas
{
    public class NotificationSagaData : ContainSagaData
    {
        public string CorrelationId { get; set; }

        public bool SubscriptionActivated { get; set; }
    }
}
