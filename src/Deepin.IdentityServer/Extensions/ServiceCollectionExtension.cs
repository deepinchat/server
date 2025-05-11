using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Deepin.Domain.Identity;
using Deepin.Infrastructure.Caching;
using Deepin.Infrastructure.Configurations;
using Deepin.Infrastructure.Data;
using Deepin.Infrastructure.Extensions;
using Duende.IdentityServer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Deepin.IdentityServer.Extensions;

internal static class ServiceCollectionExtension
{

    public static IServiceCollection AddCustomInfrastructure(
        this IServiceCollection services,
        ConnectionStringOptions connectionStrings,
        RedisCacheOptions? redisCacheOptions,
        RabbitMqOptions? rabbitMqOptions,
        Assembly[] eventConsumerAssemblies)
    {
        services
        .AddIdentityDbContext(connectionStrings.Identity ?? connectionStrings.Default)
        .AddIdentityServerPersistedGrantDbContext(connectionStrings.IdentityServer ?? connectionStrings.Default)
        .AddIdentityServerConfigurationDbContext(connectionStrings.IdentityServer ?? connectionStrings.Default);
        services.AddCaching(redisCacheOptions);
        services.AddEventBus(eventConsumerAssemblies, rabbitMqOptions);
        return services;
    }
    public static IServiceCollection AddCustomDataProtection(this IServiceCollection services, RedisCacheOptions redisCacheOptions)
    {
        if (string.IsNullOrEmpty(redisCacheOptions.ConnectionString))
        {
            throw new ArgumentNullException(nameof(RedisCacheOptions), "Redis connection string must be not null.");
        }
        services.AddDataProtection(opts =>
        {
            opts.ApplicationDiscriminator = "identity-serever";
        }).PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisCacheOptions.ConnectionString), "IdentityServer.DataProtection.Keys");
        return services;
    }
    public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
    {

        services.AddIdentity<User, Domain.Identity.Role>(options =>
        {
            // User settings.
            options.SignIn.RequireConfirmedAccount = true;
            options.User.RequireUniqueEmail = true;

            // Password settings.
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;

            options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultEmailProvider;
        })
        .AddEntityFrameworkStores<IdentityContext>();

        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.SlidingExpiration = true;
        });

        return services;
    }
    public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var idsvBuilder = services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
                options.KeyManagement.Enabled = false;
            })
            .AddConfigurationStore<ConfigurationContext>()
            .AddOperationalStore<PersistedGrantContext>(options =>
            {
                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
            })
            .AddAspNetIdentity<User>()
            .AddLicenseSummary();

        var authBuilder = services.AddAuthentication();
        if (configuration["GitHub"] is not null)
        {
            authBuilder.AddGitHub(
                configuration["GitHub:ClientId"] ?? throw new ArgumentNullException("Github ClientId must be not null"),
                configuration["GitHub:ClientSecret"] ?? throw new ArgumentNullException("Github ClientSecret must be not null"));
        }
        if (configuration["Google"] is not null)
        {
            authBuilder.AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google  
                options.ClientId = configuration["Google:ClientId"] ?? throw new ArgumentNullException("Google ClientId must be not null");
                options.ClientSecret = configuration["Google:ClientSecret"] ?? throw new ArgumentNullException("Google ClientSecret must be not null");
            });
        }


        if (environment.IsDevelopment())
        {
            idsvBuilder.AddDeveloperSigningCredential();
        }
        else
        {
            idsvBuilder.AddSigningCredential(X509CertificateLoader.LoadPkcs12FromFile(
                configuration["CERT_PATH"] ?? throw new ArgumentNullException("Certificate file path must be not null."),
                configuration["CERT_PASSWORD"] ?? throw new ArgumentNullException("Certificate file password must be not null.")));
        }
        return services;
    }
}
