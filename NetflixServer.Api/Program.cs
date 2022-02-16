using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace NetflixServer
{
    public class Program
    {
        public static Task Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseNServiceBus(ctx =>
                {
                    var connection = "Server=localhost,1433;Initial Catalog=master;User ID=sa;Password=_Netflix_123456;MultipleActiveResultSets=True;Connection Timeout=30;";
                    var endpointConfiguration = new EndpointConfiguration("Api");
                    endpointConfiguration.SendFailedMessagesTo("error");
                    endpointConfiguration.AuditProcessedMessagesTo("audit");
                    endpointConfiguration.EnableInstallers();

                    var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
                    transport.ConnectionString(connection);
                    transport.DefaultSchema("sender");
                    transport.UseSchemaForQueue("error", "dbo");
                    transport.UseSchemaForQueue("audit", "dbo");
                    transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

                    var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                    var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
                    dialect.Schema("sender");
                    persistence.ConnectionBuilder(() => new SqlConnection(connection));

                    SqlHelper.CreateSchema(connection, "sender");

                    return endpointConfiguration;
                })
                .Build()
                .RunAsync();
    }
}
