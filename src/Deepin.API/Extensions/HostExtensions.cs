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
        var appOptions = new AppOptions
        {
            ConnectionStrings = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringOptions>()!,
            Redis = builder.Configuration.GetSection("Redis").Get<RedisCacheOptions>(),
            RabbitMq = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>(),
            Smtp = builder.Configuration.GetSection("Smtp").Get<SmtpOptions>(),
            Storage = builder.Configuration.GetSection("Storage").Get<StorageOptions>(),
        };
        builder.Services
        .AddInfrastructure(appOptions, [Assembly.GetExecutingAssembly()])
        .AddApplication()
        .AddDefaultUserContexts()
        .AddCustomDataProtection(appOptions.Redis)
        .AddCustomSignalR(appOptions.Redis)
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
