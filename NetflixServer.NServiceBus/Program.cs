using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetflixServer.NServiceBus.Services;
using NetflixServer.Resources.Services;
using NetflixServer.Shared;
using NetflixServer.Shared.Commands;
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
                IConfiguration configuration = ctx.Configuration;
                var connection = configuration.GetConnectionString("DefaultConnection");
                var endpointConfiguration = new EndpointConfiguration(General.EndpointNameReceiver);
                endpointConfiguration.SendFailedMessagesTo("error");
                endpointConfiguration.AuditProcessedMessagesTo("audit");
                endpointConfiguration.EnableInstallers();

                var conventions = endpointConfiguration.Conventions();
                conventions.DefiningCommandsAs(type => type.Namespace == typeof(SubscriptionNotificationCommand).Namespace);
                endpointConfiguration.RegisterComponents(registration: configureComponent =>
                {
                    configureComponent.ConfigureComponent<IMessageSession>(_ => _endpointInstance, DependencyLifecycle.SingleInstance);
                });

                var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
                transport.ConnectionString(connection);
                transport.DefaultSchema(schemaName);
                transport.UseSchemaForQueue("error", "dbo");
                transport.UseSchemaForQueue("audit", "dbo");
                transport.UseSchemaForQueue(General.EndpointNameSender, "sender");
                transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

                transport.Routing().RouteToEndpoint(typeof(SubscriptionNotificationCommand), General.EndpointNameSender);

                var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
                dialect.Schema(schemaName);
                persistence.ConnectionBuilder(() => new SqlConnection(connection));

                SqlHelper.CreateSchema(connection, "receiver");

                return endpointConfiguration;
            });

            return builder.ConfigureServices(services =>
            {
                services.AddLogging();
                services.AddSingleton<NotificationContentService>();
                services.AddSingleton<EmailService>();
            });
        }
    }


}
