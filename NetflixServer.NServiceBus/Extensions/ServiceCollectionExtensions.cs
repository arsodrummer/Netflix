using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetflixServer.NServiceBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddLogging();
    }
}
