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
    }
}
