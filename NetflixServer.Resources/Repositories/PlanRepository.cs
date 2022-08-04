using NetflixServer.Resources.Entities;
using NetflixServer.Resources.Services;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetflixServer.Resources.Repositories
{
    public class PlanRepository
    {
        private NetflixDbService _netflixDbService;

        public PlanRepository(NetflixDbService netflixDbService)
        {
            _netflixDbService = netflixDbService;
        }

        public async Task InsertPlanAsync(string name, decimal price, string description, DateTime? expirationDate)
        {
            try
            {
                var sequenceValue = await GetNextSequenceValueAsync();

                await _netflixDbService.InsertAsync(new PlanEntity()
                {
                    PlanId = sequenceValue,
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

        public async Task<PlanEntity> GetPlanByIdAsync(long planId)
        {
            try
            {
                return await _netflixDbService.GetFirstOrDefaultAsync<PlanEntity>(new Sql($"SELECT * FROM PLANS WHERE ID = '{planId}'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<PlanEntity>> GetPlanListAsync()
        {
            try
            {
                return await _netflixDbService.GetByQueryAsync<PlanEntity>(new Sql($"SELECT * FROM PLANS"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdatePlanByIdAsync(PlanEntity planEntity)
        {
            try
            {
                await _netflixDbService.ExecuteAsync(new Sql(@$"UPDATE PLANS SET PRICE = '{planEntity.Price}' WHERE ID = '{planEntity.PlanId}'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Task<long> GetNextSequenceValueAsync() =>
            _netflixDbService.ExecuteScalarAsync<long>(new Sql($"SELECT NEXT VALUE FOR PLANS_SEQ"));
    }
}
