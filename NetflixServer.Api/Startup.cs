using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetflixServer.Api.Extensions;

namespace NetflixServer
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDatabaseServices(Configuration)
                .AddBusinessServices(Configuration)
                .AddControllers();
        }

        public void Configure(IApplicationBuilder builder, IWebHostEnvironment env)
        {
            builder
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints
                        .MapControllers();
                });
        }
    }
}
