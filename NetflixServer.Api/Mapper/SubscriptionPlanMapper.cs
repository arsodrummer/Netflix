using NetflixServer.Business.Domain;
using NetflixServer.Models.Requests;

namespace NetflixServer.Api.Mapper
{
    public static class SubscriptionPlanMapper
    {
        public static Plan ToSubscriptionPlan(this NewPlanRequest newSubscriptionPlanRequest) =>
            new Plan
            {
                Description = newSubscriptionPlanRequest.Description,
                Price = newSubscriptionPlanRequest.Price,
                Name = newSubscriptionPlanRequest.Name,
                ExpirationDate = newSubscriptionPlanRequest.ExpirationDate,
            };
    }
}
