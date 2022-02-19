using NetflixServer.Resources.Entities;
using NetflixServer.Resources.Services;
using PetaPoco;
using System;
using System.Threading.Tasks;

namespace NetflixServer.Resources.Repositories
{
    public class SubscriberRepository
    {
        private NetflixDbService _netflixDbService;

        public SubscriberRepository(NetflixDbService netflixDbService)
        {
            _netflixDbService = netflixDbService;
        }

        public async Task<long> InsertSubscriberAsync(string email, string userName, bool active)
        {
            try
            {
                var sequenceValue = await GetNextSequenceValueAsync();

                await _netflixDbService.InsertAsync(new SubscriberEntity()
                {
                    SubscriberId = sequenceValue,
                    Email = email,
                    UserName = userName,
                    Active = active,
                });

                return sequenceValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SubscriberEntity> GetSubscriberByIdAsync(long subscriberId)
        {
            try
            {
                return await _netflixDbService.GetFirstOrDefaultAsync<SubscriberEntity>(new Sql($"SELECT * FROM SUBSCRIBER WHERE ID_SUBSCRIBER = '{subscriberId}'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateSubscriberByIdAsync(SubscriberEntity subscriberEntity)
        {
            try
            {
                await _netflixDbService.ExecuteAsync(new Sql($"UPDATE SUBSCRIBER SET ID_SUBSCRIPTION_PLAN = '{subscriberEntity.SubscriptionPlanId}' WHERE ID_SUBSCRIBER = '{subscriberEntity.SubscriberId}'"));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private Task<long> GetNextSequenceValueAsync() =>
            _netflixDbService.ExecuteScalarAsync<long>(new Sql($"SELECT NEXT VALUE FOR SUBSCRIBER_SEQ"));
    }
}
