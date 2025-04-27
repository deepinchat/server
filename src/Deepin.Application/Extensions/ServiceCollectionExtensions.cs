using Deepin.Application.Queries.Chats;
using Deepin.Application.Queries.Files;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Deepin.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        services
        .AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        })
        .AddValidatorsFromAssembly(assembly)
        .AddAutoMapper(assembly)
        .AddQueries();
        return services;
    }
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IChatQueries, ChatQueries>();
        services.AddScoped<IFileQueries, FileQueries>();
        return services;
    }
}
