using System.Reflection;
using Deepin.API.Hubs;
using Deepin.Application.Extensions;
using Deepin.Infrastructure.Caching;
using Deepin.Infrastructure.Configurations;
using Deepin.Infrastructure.Data;
using Deepin.Infrastructure.Extensions;

namespace Deepin.API.Extensions;

public static class HostExtensions
{
    public static WebApplicationBuilder AddApplicationService(this WebApplicationBuilder builder)
    {
        var redisOptions = builder.Configuration.GetSection("Redis").Get<RedisCacheOptions>();
        builder.Services
        .AddInfrastructure(
            connectionStringOptions: builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringOptions>() ?? throw new ArgumentNullException("ConnectionStrings must be not null."),
            redisCacheOptions: redisOptions,
            smtpOptions: builder.Configuration.GetSection("Smtp").Get<SmtpOptions>(),
            storageOptions: builder.Configuration.GetSection("Storage").Get<StorageOptions>(),
            rabbitMqOptions: builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>(),
            eventConsumerAssemblies: [Assembly.GetExecutingAssembly()])
        .AddApplication()
        .AddDefaultUserContexts()
        .AddCustomDataProtection(redisOptions)
        .AddCustomSignalR(redisOptions)
        .AddMigration<ChatDbContext>()
        .AddMigration<MessageDbContext>()
        .AddMigration<NotificationDbContext>()
        .AddMigration<ContactDbContext>()
        .AddMigration<StorageDbContext>();

        builder.AddServiceDefaults();
        return builder;
    }
    public static WebApplication UseApplicationService(this WebApplication app)
    {
        app.UseServiceDefaults();

        app.MapHub<ChatsHub>("/hub/chats");
        app.MapGet("/api/about", () => new
        {
            Name = "Deepin.API",
            Version = "1.0.0",
            DeepinEnv = app.Configuration["DEEPIN_ENV"],
            app.Environment.EnvironmentName
        });
        return app;
    }
}
