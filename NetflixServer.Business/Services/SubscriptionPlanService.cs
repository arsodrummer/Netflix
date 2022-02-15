using NetflixServer.Business.Domain;
using NetflixServer.Business.Interfaces;
using NetflixServer.Resources.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        public SubscriptionPlanRepository _subscriptionPlanRepository;

        public SubscriptionPlanService(SubscriptionPlanRepository subscriptionPlanRepository)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
        }

        public async Task CreateSubscriptionPlanAsync(SubscriptionPlan subscriptionPlan, CancellationToken cancellationToken)
        {
            await _subscriptionPlanRepository.InsertSubscriptionPlanAsync(subscriptionPlan.Name, subscriptionPlan.Price, subscriptionPlan.Description);
        }

        public async Task<SubscriptionPlan> GetSubscriptionPlanByIdAsync(long subscriptionPlanId, CancellationToken cancellationToken)
        {
            var subscriptionPlanEntity = await _subscriptionPlanRepository.GetSubscriptionPlanByIdAsync(subscriptionPlanId);

            if (subscriptionPlanEntity == null)
            {
                return null;
            }

            return new SubscriptionPlan
            {
                SubscriptionPlanId = subscriptionPlanEntity.SubscriptionPlanId,
                Description = subscriptionPlanEntity.Description,
                Name = subscriptionPlanEntity.Name,
                Price = subscriptionPlanEntity.Price,
            };
        }
    }
}
