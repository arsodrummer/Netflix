using NetflixServer.Resources.Entities;
using NetflixServer.Resources.Services;
using PetaPoco;
using System;
using System.Threading.Tasks;

namespace NetflixServer.Resources.Repositories
{
    public class SubscriptionPlanRepository
    {
        private NetflixDbService _netflixDbService;

        public SubscriptionPlanRepository(NetflixDbService netflixDbService)
        {
            _netflixDbService = netflixDbService;
        }

        public async Task InsertSubscriptionPlanAsync(string name, decimal price, string description, DateTime? expirationDate)
        {
            try
            {
                var sequenceValue = await GetNextSequenceValueAsync();

                await _netflixDbService.InsertAsync(new SubscriptionPlanEntity()
                {
                    SubscriptionPlanId = sequenceValue,
                    Description = description,
                    Name = name,
                    Price = price,
                    ExpirationDate = expirationDate,
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SubscriptionPlanEntity> GetSubscriptionPlanByIdAsync(long subscriptionPlanId)
        {
            try
            {
                return await _netflixDbService.GetFirstOrDefaultAsync<SubscriptionPlanEntity>(new Sql($"SELECT * FROM SUBSCRIPTION_PLAN WHERE ID_SUBSCRIPTION_PLAN = '{subscriptionPlanId}'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateSubscriptionPlanByIdAsync(SubscriptionPlanEntity subscriptionPlanEntity)
        {
            try
            {
                await _netflixDbService.ExecuteAsync(new Sql(@$"UPDATE SUBSCRIPTION_PLAN 
                                                                SET EXPIRATION_DATE = '{subscriptionPlanEntity.ExpirationDate}', PRICE = '{subscriptionPlanEntity.Price}'
                                                                WHERE ID_SUBSCRIPTION_PLAN = '{subscriptionPlanEntity.SubscriptionPlanId}'"));
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
