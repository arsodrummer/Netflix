using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NetflixServer.Shared;
using NServiceBus;
using System.Threading.Tasks;

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
                    IConfiguration configuration = ctx.Configuration;
                    var connection = configuration.GetConnectionString("DefaultConnection");
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
