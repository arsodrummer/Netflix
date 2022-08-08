using NetflixServer.Api.Models.Requests;
using NetflixServer.Business.Domain;

namespace NetflixServer.Api.Mapper
{
    public static class SubscriptionMapper
    {
        public static Subscription ToSubscription(this NewSubscriptionRequest subscription) =>
            new Subscription()
            {
                UserId = subscription.UserId,
                PlanId = subscription.PlanId,
                ExpirationDate = subscription.ExpirationDate,
            };
    }
}
