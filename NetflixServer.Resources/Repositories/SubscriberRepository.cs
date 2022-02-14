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

        public async Task InsertSubscriberAsync(string email, string userName)
        {
            try
            {
                var sequenceValue = await GetNextSequenceValueAsync();

                await _netflixDbService.InsertAsync(new SubscriberEntity()
                {
                    SubscriberId = sequenceValue,
                    Email = email,
                    UserName = userName,
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SubscriberEntity> GetSubscriberByIdAsync(string subscriberId)
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

        private Task<long> GetNextSequenceValueAsync() =>
            _netflixDbService.ExecuteScalarAsync<long>(new Sql($"SELECT current_value FROM sys.sequences WHERE name = 'SUBSCRIBER_SEQ'"));
    }
}
