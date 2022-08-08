using NetflixServer.Business.Interfaces;
using NetflixServer.Resources.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetflixServer.Business.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        public SubscriptionRepository _subscriptionRepository;

        public SubscriptionService(SubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task CreateSubscriptionAsync(long userId, long planId, DateTime expirationDate, CancellationToken cancellationToken)
        {
            await _subscriptionRepository.InsertSubscriptionAsync(userId, planId, expirationDate);
        }
    }
}
