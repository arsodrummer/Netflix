using NetflixServer.Business.Domain;
using NetflixServer.Models.Requests;

namespace NetflixServer.Api.Mapper
{
    public static class PlanMapper
    {
        public static Plan ToPlan(this NewPlanRequest newPlanRequest) =>
            new Plan
            {
                Description = newPlanRequest.Description,
                Price = newPlanRequest.Price,
                Name = newPlanRequest.Name,
                ExpirationDate = newPlanRequest.ExpirationDate,
            };
    }
}
