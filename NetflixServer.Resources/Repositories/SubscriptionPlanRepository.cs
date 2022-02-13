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

        public async Task InsertSubscriptionPlanAsync(string name, decimal price, string description)
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
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Task<long> GetNextSequenceValueAsync() =>
            _netflixDbService.ExecuteScalarAsync<long>(new Sql($"SELECT current_value FROM sys.sequences WHERE name = 'SUBSCRIPTION_PLAN_SEQ'"));
    }
}
