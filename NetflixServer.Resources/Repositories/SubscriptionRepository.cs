using NetflixServer.Resources.Entities;
using NetflixServer.Resources.Services;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetflixServer.Resources.Repositories
{
    public class SubscriptionRepository
    {
        private NetflixDbService _netflixDbService;

        public SubscriptionRepository(NetflixDbService netflixDbService)
        {
            _netflixDbService = netflixDbService;
        }

        public async Task<long> InsertSubscriptionAsync(long userId, long planId, DateTime expirationDate)
        {
            try
            {
                var sequenceValue = await GetNextSequenceValueAsync();

                await _netflixDbService.InsertAsync(new SubscriptionEntity()
                {
                    SubscriptionId = sequenceValue,
                    UserId = userId,
                    PlanId = planId,
                    ExpirationDate = expirationDate,
                });

                return sequenceValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<SubscriptionEntity>> GetSubscriptionListAsync()
        {
            try
            {
                return await _netflixDbService.GetByQueryAsync<SubscriptionEntity>(new Sql($"SELECT * FROM SUBSCRIPTIONS"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Task<long> GetNextSequenceValueAsync() =>
            _netflixDbService.ExecuteScalarAsync<long>(new Sql($"SELECT NEXT VALUE FOR SUBSCRIPTION_PLAN_SEQ"));
    }
}
