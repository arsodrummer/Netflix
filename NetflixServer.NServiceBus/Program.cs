using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetflixServer.NServiceBus.Services;
using NetflixServer.Resources.Services;
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
                IConfiguration configuration = ctx.Configuration;
                var connection = configuration.GetConnectionString("DefaultConnection");
                var endpointConfiguration = new EndpointConfiguration("NServiceBus");
                endpointConfiguration.SendFailedMessagesTo("error");
                endpointConfiguration.AuditProcessedMessagesTo("audit");
                endpointConfiguration.EnableInstallers();

                var conventions = endpointConfiguration.Conventions();
                //conventions.DefiningEventsAs(type => type.Name == typeof(SendEmailCommand).Name);
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

            return builder.ConfigureServices(services =>
            {
                services.AddLogging();
                services.AddSingleton<NotificationContentService>();
                services.AddSingleton<EmailService>();
            });
        }
    }


}
