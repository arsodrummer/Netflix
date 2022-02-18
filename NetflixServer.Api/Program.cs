using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NetflixServer.Shared;
using NServiceBus;

namespace NetflixServer
{
    public class Program
    {
        private static IEndpointInstance _endpointInstance;

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
                    transport.DefaultSchema("receiver");
                    transport.UseSchemaForQueue("error", "dbo");
                    transport.UseSchemaForQueue("audit", "dbo");

                    var subscriptions = transport.SubscriptionSettings();
                    subscriptions.SubscriptionTableName(
                        tableName: "Subscriptions",
                        schemaName: "dbo");

                    SqlHelper.CreateSchema(connection, "sender");

                    return endpointConfiguration;
                })
                .Build()
                .RunAsync();
    }
}
