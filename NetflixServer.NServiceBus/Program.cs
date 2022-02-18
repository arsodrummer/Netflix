using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetflixServer.Shared;
using NServiceBus;
using NServiceBus.Persistence.Sql;

[assembly: SqlPersistenceSettings(
    MsSqlServerScripts = true)]

namespace NetflixServer.NServiceBus
{
    public class Program
    {
        private static IEndpointInstance _endpointInstance;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args, "receiver").Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string schemaName)
        {
            var builder = Host.CreateDefaultBuilder(args);

            builder.UseNServiceBus(ctx =>
            {
                var connection = "Server=localhost,1433;Initial Catalog=master;User ID=sa;Password=_Netflix_123456;MultipleActiveResultSets=True;Connection Timeout=30;";
                var endpointConfiguration = new EndpointConfiguration("NServiceBus");
                endpointConfiguration.SendFailedMessagesTo("error");
                endpointConfiguration.AuditProcessedMessagesTo("audit");
                endpointConfiguration.EnableInstallers();

                var conventions = endpointConfiguration.Conventions();
                //conventions.DefiningEventsAs(type => type.Namespace == typeof(EmailEvent).Namespace);
                conventions.DefiningCommandsAs(type => type.Namespace == typeof(NotificationCommand).Namespace);
                endpointConfiguration.RegisterComponents(registration: configureComponent =>
                {
                    configureComponent.ConfigureComponent<IMessageSession>(_ => _endpointInstance, DependencyLifecycle.SingleInstance);
                });

                var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
                transport.ConnectionString(connection);
                transport.DefaultSchema(schemaName);
                transport.UseSchemaForQueue("error", "dbo");
                transport.UseSchemaForQueue("audit", "dbo");
                transport.UseSchemaForQueue("Api", "sender");
                transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

                transport.Routing().RouteToEndpoint(typeof(NotificationCommand), "Api");

                var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
                dialect.Schema(schemaName);
                persistence.ConnectionBuilder(() => new SqlConnection(connection));

                //endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
                SqlHelper.CreateSchema(connection, "receiver");

                return endpointConfiguration;
            });

            return builder.ConfigureServices(services => { services.AddLogging(); });
        }
    }


}
