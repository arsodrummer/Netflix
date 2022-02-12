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
                    Email = email,
                    UserName = userName,
                    SubscriberId = sequenceValue,
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Task<long> GetNextSequenceValueAsync() =>
            _netflixDbService.ExecuteScalarAsync<long>(new Sql($"SELECT SUBSCRIBER.NEXTVAL FROM DUAL"));
    }
}
