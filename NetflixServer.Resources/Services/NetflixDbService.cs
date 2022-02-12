using PetaPoco;

namespace NetflixServer.Resources.Services
{
    public class NetflixDbService : DbService
    {
        private readonly IDatabase _database;

        public NetflixDbService(IDatabase database) : base(database)
        {
            _database = database;
        }

        public void Ping() => _database.ExecuteScalarAsync<object>("SELECT * FROM v$version");
    }
}
