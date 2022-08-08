using NetflixServer.Business.Interfaces;
using NetflixServer.Business.Models.Responses;
using NetflixServer.Resources.Repositories;
using System;
using System.Collections.Generic;
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

        public async Task<List<SubscriptionByIdResponse>> GetSubscriptionListAsync(CancellationToken cancellationToken)
        {
            var res = await _subscriptionRepository.GetSubscriptionListAsync();

            List<SubscriptionByIdResponse> listOfPlans = new List<SubscriptionByIdResponse>();

            foreach (var item in res)
            {
                listOfPlans.Add(new SubscriptionByIdResponse
                {
                    Id = item.SubscriptionId,
                    UserId = item.UserId,
                    PlanId = item.PlanId,
                    ExpirationDate = item.ExpirationDate,
                });
            }

            return listOfPlans;
        }
    }
}
