using System.Reflection;
using Deepin.Application.Extensions;
using Deepin.IdentityServer.Setup;
using Deepin.Infrastructure.Caching;
using Deepin.Infrastructure.Configurations;
using Deepin.Infrastructure.Data;
using Deepin.Infrastructure.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

namespace Deepin.IdentityServer.Extensions;

public static class HostingExtensions
{
    public static WebApplicationBuilder AddApplicationService(this WebApplicationBuilder builder)
    {
        var redisOptions = builder.Configuration.GetSection("Redis").Get<RedisCacheOptions>();
        builder.Services
        .AddCustomInfrastructure(
            connectionStrings: builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringOptions>() ?? throw new ArgumentNullException("ConnectionStrings must be not null."),
            redisCacheOptions: redisOptions,
            rabbitMqOptions: builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>(),
            eventConsumerAssemblies: [Assembly.GetExecutingAssembly()])
        .AddApplication()
        .AddCustomIdentity()
        .AddCustomIdentityServer(builder.Configuration, builder.Environment);

        builder.Services.AddRazorPages();
        if (redisOptions is not null)
        {
            builder.Services.AddCustomDataProtection(redisOptions);
        }
        builder.Services
            .AddMigration<IdentityContext>()
            .AddMigration<ConfigurationContext>((db, sp) => new ConfigurationDbSeeder(db).SeedAsync())
            .AddMigration<PersistedGrantContext>();
        return builder;
    }

    public static WebApplication UseApplicationService(this WebApplication app)
    {// Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapStaticAssets();

        app.MapHealthChecks("health");

        app.MapRazorPages().RequireAuthorization().WithStaticAssets();
        return app;
    }
}
