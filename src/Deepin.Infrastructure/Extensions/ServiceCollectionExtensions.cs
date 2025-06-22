using Deepin.Application.Interfaces;
using Deepin.Domain.ChatAggregate;
using Deepin.Domain.Emails;
using Deepin.Domain.FileAggregate;
using Deepin.Domain.MessageAggregate;
using Deepin.Infrastructure.Caching;
using Deepin.Infrastructure.Configurations;
using Deepin.Infrastructure.Data;
using Deepin.Infrastructure.EventBus;
using Deepin.Infrastructure.FileStorage;
using Deepin.Infrastructure.Repositories;
using Deepin.Infrastructure.Services;
using Duende.IdentityServer.EntityFramework.Storage;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Reflection;

namespace Deepin.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
    ConnectionStringOptions connectionStringOptions,
    RedisCacheOptions? redisCacheOptions,
    SmtpOptions? smtpOptions,
    StorageOptions? storageOptions,
    RabbitMqOptions? rabbitMqOptions,
    Assembly[] eventConsumerAssemblies)
    {
        if (connectionStringOptions is not null)
        {
            services.AddDbContexts(connectionStringOptions);
        }
        if (redisCacheOptions is not null)
        {
            services.AddCaching(redisCacheOptions);
        }
        if (smtpOptions is not null)
        {
            services.AddEmailSender(smtpOptions);
        }
        if (storageOptions is not null)
        {
            services.AddStorageProvider(storageOptions);
        }

        services.AddEventBus(eventConsumerAssemblies, rabbitMqOptions);
        return services;
    }
    public static IServiceCollection AddEmailSender(this IServiceCollection services, SmtpOptions? smtpOptions = null)
    {
        if (smtpOptions is null || smtpOptions.IsEnabled == false)
        {
            smtpOptions = new SmtpOptions();
            services.AddSingleton<IEmailSender, FakeEmailSender>();
        }
        else
        {
            services.AddSingleton<IEmailSender>(sp => new SmtpEmailSender(sp.GetRequiredService<ILogger<SmtpEmailSender>>(), smtpOptions));
        }
        services.AddSingleton(smtpOptions);
        return services;
    }
    public static IServiceCollection AddCaching(this IServiceCollection services, RedisCacheOptions? options = null)
    {
        if (options is null)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheManager>(sp => new MemoryCacheManager(sp.GetRequiredService<IMemoryCache>(), new CacheOptions()));
        }
        else
        {
            services.AddSingleton(options);
            services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(options.ConnectionString));
            services.AddSingleton<ICacheManager, RedisCacheManager>();
        }
        return services;
    }
    public static IServiceCollection AddDbContexts(this IServiceCollection services, ConnectionStringOptions connectionStrings)
    {
        if (connectionStrings is null)
        {
            throw new ArgumentNullException(nameof(connectionStrings), "Connection strings cannot be null");
        }

        if (!string.IsNullOrWhiteSpace(connectionStrings.Chat))
        {
            services.AddChatDbContext(connectionStrings.Chat);
        }
        if (!string.IsNullOrWhiteSpace(connectionStrings.Contact))
        {
            services.AddContactDbContext(connectionStrings.Contact);
        }
        if (!string.IsNullOrWhiteSpace(connectionStrings.Storage))
        {
            services.AddStorageDbContext(connectionStrings.Storage);
        }
        if (!string.IsNullOrWhiteSpace(connectionStrings.Notification))
        {
            services.AddNotificationDbContext(connectionStrings.Notification);
        }
        if (!string.IsNullOrWhiteSpace(connectionStrings.Message))
        {
            services.AddMessageDbContext(connectionStrings.Message);
        }
        if (!string.IsNullOrWhiteSpace(connectionStrings.Identity))
        {
            services.AddIdentityDbContext(connectionStrings.Identity);
        }
        if (!string.IsNullOrWhiteSpace(connectionStrings.IdentityServer))
        {
            services.AddIdentityServerPersistedGrantDbContext(connectionStrings.IdentityServer);
            services.AddIdentityServerConfigurationDbContext(connectionStrings.IdentityServer);
        }

        services.AddScoped<IDbConnectionFactory>(sp => new NpgsqlDbConnectionFactory(connectionStrings));
        return services;
    }
    public static IServiceCollection AddChatDbContext(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Chat DbConnection string cannot be null or empty", nameof(connectionString));
        }
        services.AddDbContext<ChatDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IDirectChatRepository, DirectChatRepository>();
        services.AddScoped<IGroupChatRepository, GroupChatRepository>();
        services.AddScoped<IChatReadStatusRepository, ChatReadStatusRepository>();
        services.AddScoped<IChatSettingsRepository, ChatSettingsRepository>();
        return services;
    }
    public static IServiceCollection AddContactDbContext(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Contact DbConnection string cannot be null or empty", nameof(connectionString));
        }
        services.AddDbContext<ContactDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        // services.AddScoped<IContactRepository, Contactr>();
        return services;
    }
    public static IServiceCollection AddStorageDbContext(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Storage DbConnection string cannot be null or empty", nameof(connectionString));
        }
        services.AddDbContext<StorageDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IFileRepository, FileRepository>();
        return services;
    }
    public static IServiceCollection AddNotificationDbContext(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Notification DbConnection string cannot be null or empty", nameof(connectionString));
        }
        services.AddDbContext<NotificationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IEmailRepository, EmailRepository>();
        return services;
    }
    public static IServiceCollection AddMessageDbContext(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Message DbConnection string cannot be null or empty", nameof(connectionString));
        }
        services.AddDbContext<MessageDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IMessageReactionRepository, MessageReactionRepository>();
        return services;
    }
    public static IServiceCollection AddEventBus(this IServiceCollection services, Assembly[] eventConsumerAssemblies, RabbitMqOptions? mqOptions = null)
    {
        services.AddScoped<IEventBus, IntegrationEventBus>();
        if (mqOptions is null)
        {
            services.AddEventBusInMemory(eventConsumerAssemblies);
        }
        else
        {
            services.AddEventBusRabbitMQ(mqOptions, eventConsumerAssemblies);
        }
        return services;
    }
    public static IServiceCollection AddEventBusInMemory(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumers(assemblies);
            cfg.UsingInMemory((ctx, cfg) =>
            {
                cfg.ConfigureEndpoints(ctx);
            });
        });
        return services;
    }
    public static IServiceCollection AddEventBusRabbitMQ(this IServiceCollection services, RabbitMqOptions mqConfig, Assembly[] assemblies)
    {
        if (mqConfig is null)
        {
            throw new ArgumentNullException(nameof(mqConfig), "RabbitMQ configuration cannot be null");
        }
        services.AddMassTransit(config =>
        {
            config.AddConsumers(assemblies);
            config.UsingRabbitMq((ctx, mq) =>
            {
                mq.Host(mqConfig.Host, mqConfig.Port, mqConfig.VirtualHost, h =>
                {
                    h.Username(mqConfig.Username);
                    h.Password(mqConfig.Password);

                });
                mq.ReceiveEndpoint(mqConfig.QueueName, x =>
                {
                    x.ConfigureConsumers(ctx);
                });
            });
        });
        return services;
    }
    public static IServiceCollection AddStorageProvider(this IServiceCollection services, StorageOptions storageOptions)
    {
        if (storageOptions.Provider == StorageProvider.Local)
        {
            services.AddSingleton<IFileStorage>(sp => new LocalFileStorage(storageOptions));
        }
        else if (storageOptions.Provider == StorageProvider.AwsS3)
        {
            services.AddSingleton<IFileStorage>(sp => new S3FileStorage(storageOptions));
        }
        else
        {
            throw new NotSupportedException($"The storage provider '{storageOptions.Provider}' is not supported.");
        }
        return services;
    }
    public static IServiceCollection AddIdentityDbContext(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("IdentityServer DbConnection string cannot be null or empty", nameof(connectionString));
        }
        services.AddDbContext<IdentityContext>(options =>
        {
            options.UseNpgsql(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(3);
            });
        }, ServiceLifetime.Scoped);
        return services;
    }
    public static IServiceCollection AddIdentityServerPersistedGrantDbContext(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("PersistedGrant DbConnection string cannot be null or empty", nameof(connectionString));
        }
        services.AddOperationalDbContext<PersistedGrantContext>(options =>
        {
            options.ConfigureDbContext = builder => builder.UseNpgsql(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(3);
            });
        });
        return services;
    }
    public static IServiceCollection AddIdentityServerConfigurationDbContext(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Configuration DbConnection string cannot be null or empty", nameof(connectionString));
        }
        services.AddConfigurationDbContext<ConfigurationContext>(options =>
        {
            options.ConfigureDbContext = builder => builder.UseNpgsql(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(3);
            });
        });
        return services;
    }
}