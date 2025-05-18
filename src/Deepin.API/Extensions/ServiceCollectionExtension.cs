using Deepin.Infrastructure.Caching;
using Deepin.API.Services;
using StackExchange.Redis;
using Microsoft.AspNetCore.DataProtection;
using Deepin.Application.Interfaces;

namespace Deepin.API.Extensions;

public static class ServiceCollectionExtension
{
    public static WebApplicationBuilder AddServiceDefaults(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDefaultControllers()
            .AddDefaultCorsPolicy()
            .AddDefaultHealthChecks()
            .AddDefaultAuthentication(builder.Configuration)
            .AddDefaultOpenApi(builder.Configuration)
            .AddApiServices();

        return builder;
    }
    private static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        return services.AddScoped<IChatService, ChatService>();
    }
    public static IServiceCollection AddDefaultUserContexts(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<IUserContext, HttpUserContext>();
        return services;
    }

    public static IServiceCollection AddCustomDataProtection(this IServiceCollection services, RedisCacheOptions? redis = null)
    {
        if (redis is null)
        {
            services.AddDataProtection(opts =>
            {
                opts.ApplicationDiscriminator = "Deepin.API";
            });
        }
        else
        {
            services.AddDataProtection(opts =>
            {
                opts.ApplicationDiscriminator = "Deepin.API";
            })
             .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redis.ConnectionString), "Deepin.API.DataProtection.Keys");
        }
        return services;
    }
    public static IServiceCollection AddCustomSignalR(this IServiceCollection services, RedisCacheOptions? redis = null)
    {
        if (redis is null)
        {
            services.AddSignalR();
        }
        else
        {
            services.AddSignalR().AddStackExchangeRedis(redis.ConnectionString, options => { });
        }
        return services;
    }
    public static IServiceCollection AddDefaultCorsPolicy(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy("allow_any",
                builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });
    }
}
