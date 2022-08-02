using NetflixServer.Resources.Entities;
using NetflixServer.Resources.Services;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetflixServer.Resources.Repositories
{
    public class UserRepository
    {
        private NetflixDbService _netflixDbService;

        public UserRepository(NetflixDbService netflixDbService)
        {
            _netflixDbService = netflixDbService;
        }

        public async Task<long> InsertUserAsync(string email, string userName, bool active)
        {
            try
            {
                var sequenceValue = await GetNextSequenceValueAsync();

                await _netflixDbService.InsertAsync(new UserEntity()
                {
                    UserId = sequenceValue,
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

        public async Task<UserEntity> GetUserByIdAsync(long userId)
        {
            try
            {
                return await _netflixDbService.GetFirstOrDefaultAsync<UserEntity>(new Sql($"SELECT * FROM USERS WHERE ID = '{userId}'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<UserEntity>> GetUserListAsync()
        {
            try
            {
                return await _netflixDbService.GetByQueryAsync<UserEntity>(new Sql($"SELECT * FROM USERS"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateUserByIdAsync(UserEntity userEntity)
        {
            try
            {
                await _netflixDbService.ExecuteAsync(new Sql($"UPDATE USERS SET ID_SUBSCRIPTION_PLAN = '{userEntity.UserId}' WHERE ID = '{userEntity.UserId}'")); // TODO: fix this query
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private Task<long> GetNextSequenceValueAsync() =>
            _netflixDbService.ExecuteScalarAsync<long>(new Sql($"SELECT NEXT VALUE FOR USERS_SEQ"));
    }
}
