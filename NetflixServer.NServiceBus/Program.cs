using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Persistence.Sql;

[assembly: SqlPersistenceSettings(
    MsSqlServerScripts = true)]

namespace NetflixServer.NServiceBus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);

            builder.UseNServiceBus(ctx =>
            {
                var connection = "Server=localhost,1433;Initial Catalog=master;User ID=sa;Password=_Netflix_123456;MultipleActiveResultSets=True;Connection Timeout=30;";
                var endpointConfiguration = new EndpointConfiguration("NServiceBus");
                var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
                transport.ConnectionString(connection);
                //transport.DefaultSchema("receiver");
                transport.UseSchemaForQueue("error", "dbo");
                transport.UseSchemaForQueue("audit", "dbo");
                transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

                var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
                dialect.Schema("receiver");
                persistence.ConnectionBuilder(() => new SqlConnection(connection));

                //endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
                endpointConfiguration.EnableInstallers("sa");
                return endpointConfiguration;
            });

            return builder.ConfigureServices(services => { services.AddLogging(); });
        }
    }


}
