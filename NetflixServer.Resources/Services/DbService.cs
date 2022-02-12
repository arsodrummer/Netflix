using PetaPoco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetflixServer.Resources.Services
{
    public class DbService
    {
        private readonly IDatabase _database;

        protected DbService(IDatabase database)
        {
            _database = database;
        }

        public Task<List<T>> GetByQueryAsync<T>(Sql query) =>
            _database.FetchAsync<T>(query);

        public Task<T> GetSingleOrDefaultByQueryAsync<T>(Sql query) =>
            _database.SingleOrDefaultAsync<T>(query);

        public T GetSingleOrDefaultByQuery<T>(Sql query) =>
            _database.SingleOrDefault<T>(query);

        public Task<T> GetFirstOrDefaultAsync<T>(Sql query) =>
            _database.FirstOrDefaultAsync<T>(query);

        public Task<object> InsertAsync<T>(T entity) =>
            _database.InsertAsync(entity);

        public Task<int> UpdateAsync<T>(T entity) =>
            _database.UpdateAsync(entity);

        public Task<int> ExecuteAsync(Sql sql) =>
            _database.ExecuteAsync(sql);

        public Task<T> ExecuteScalarAsync<T>(Sql sql) =>
            _database.ExecuteScalarAsync<T>(sql);

        public ITransaction CreateTransaction() =>
            _database.GetTransaction();

        public void AbortTransaction() =>
            _database.AbortTransaction();
    }
}
