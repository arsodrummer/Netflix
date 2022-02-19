using NetflixServer.Business.Domain;
using NetflixServer.Models.Requests;

namespace NetflixServer.Api.Mapper
{
    public static class SubscriptionPlanMapper
    {
        public static SubscriptionPlan ToSubscriptionPlan(this NewSubscriptionPlanRequest newSubscriptionPlanRequest) =>
            new SubscriptionPlan
            {
                Description = newSubscriptionPlanRequest.Description,
                Price = newSubscriptionPlanRequest.Price,
                Name = newSubscriptionPlanRequest.Name,
                ExpirationDate = newSubscriptionPlanRequest.ExpirationDate,
            };
    }
}
