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
                //.AddInfrastructureServices(Configuration)
                //.AddApiServices()
                .AddDatabaseServices(Configuration)
                .AddBusinessServices(Configuration)
                //.AddApiHost(options =>
                //{
                //    options.Name = "Netflix";
                //    options.Domain = "Back";
                //    //options.EnableLegacyMode = true;
                //})
                .AddControllers();
                //.AddFluentValidation(fv =>
                //{
                //    //fv.RegisterValidatorsFromAssembly(typeof(Startup).Assembly,
                //    //    filter: asr => asr.ValidatorType != typeof(UserClaimsValidator),
                //    //    lifetime: ServiceLifetime.Singleton);
                //    //fv.RunDefaultMvcValidationAfterFluentValidationExecutes = true;
                //    //fv.LocalizationEnabled = true;
                //    fv.ValidatorOptions.DisplayNameResolver = (type, member, expression) =>
                //        member == null ? "request" : member.Name;
                //});
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
