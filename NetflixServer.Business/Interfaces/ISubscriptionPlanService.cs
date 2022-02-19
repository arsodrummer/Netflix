using NetflixServer.Business.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task CreateSubscriptionPlanAsync(SubscriptionPlan subscriptionPlan, CancellationToken cancellationToken);

        Task<SubscriptionPlan> GetSubscriptionPlanByIdAsync(long subscriptionPlanId, CancellationToken cancellationToken);

        Task<SubscriptionPlan> UpdateSubscriptionPlanById(long subscriptionPlanId, decimal price, DateTime? expirationDate, CancellationToken cancellationToken);
    }
}
