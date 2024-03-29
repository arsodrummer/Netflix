﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetflixServer.Business.Interfaces;
using NetflixServer.Business.Services;
using NetflixServer.Resources.Repositories;
using NetflixServer.Resources.Services;
using PetaPoco;
using PetaPoco.Providers;
using System.Data;

namespace NetflixServer.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddTransient<UserRepository>()
                .AddTransient<PlanRepository>()
                .AddTransient<SubscriptionRepository>()
                .AddTransient<NetflixDbService>(sp =>
                {
                    var database = sp.GetService<IDatabaseBuildConfiguration>()
                        .UsingConnectionString(configuration.GetConnectionString("DefaultConnection"))
                        .Create();

                    return new NetflixDbService(database);
                })
                .AddSingleton<IDatabaseBuildConfiguration>(sp =>
                {
                    var logger = sp.GetService<ILogger<DatabaseConfiguration>>();
                    return DatabaseConfiguration.Build()
                        .UsingProvider<SqlServerDatabaseProvider>()
                        .UsingIsolationLevel(IsolationLevel.ReadCommitted)
                        .UsingCommandExecuted((sender, args) => { logger.LogInformation($"{args.Command.CommandText}"); });
                });

        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddScoped<IUserService, UserService>()
                .AddScoped<IPlanService, PlanService>()
                .AddScoped<ISubscriptionService, SubscriptionService>()
                .AddTransient<MessageService>();
    }
}
