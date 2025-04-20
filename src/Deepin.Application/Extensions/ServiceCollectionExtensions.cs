using Deepin.Application.Queries.Chats;
using Deepin.Infrastructure.Caching;
using Deepin.Infrastructure.Configurations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Deepin.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, AppOptions appOptions)
    {
        if (appOptions is null)
        {
            throw new ArgumentNullException(nameof(appOptions), "AppOptions cannot be null");
        }
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        services
        .AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        })
        .AddValidatorsFromAssembly(assembly)
        .AddAutoMapper(assembly)
        .AddQueries(appOptions.ConnectionStrings);
        return services;
    }
    private static IServiceCollection AddQueries(this IServiceCollection services, ConnectionStringOptions connectionStrings)
    {
        if (connectionStrings is null)
        {
            throw new ArgumentNullException(nameof(connectionStrings), "Connection strings cannot be null");
        }
        services.AddScoped<IChatQueries>(sp => new ChatQueries(connectionStrings.Conversation ?? connectionStrings.Default, sp.GetRequiredService<ICacheManager>()));
        return services;
    }
}
