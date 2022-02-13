using NetflixServer.Business.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task CreateSubscriptionPlanAsync(SubscriptionPlan subscriptionPlan, CancellationToken cancellationToken);
    }
}
