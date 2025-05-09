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
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, AppOptions appOptions, Assembly[] eventConsumerAssemblies)
    {
        services.AddSingleton(appOptions);
        services.AddDbContexts(appOptions.ConnectionStrings);
        services.AddCaching(appOptions.Redis);
        services.AddEmailSender(appOptions.Smtp);
        services.AddStorageProvider(appOptions.Storage ?? throw new ArgumentNullException(nameof(appOptions.Storage), "Storage options cannot be null"));
        services.AddEventBus(eventConsumerAssemblies, appOptions.RabbitMq);
        return services;
    }
    public static IServiceCollection AddEmailSender(this IServiceCollection services, SmtpOptions? smtpOptions = null)
    {
        if (smtpOptions is null || smtpOptions.IsEnabled == false)
        {
            services.AddSingleton<IEmailSender, FakeEmailSender>();
        }
        else
        {
            services.AddSingleton<IEmailSender>(sp => new SmtpEmailSender(sp.GetRequiredService<ILogger<SmtpEmailSender>>(), smtpOptions));
        }
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

        AddChatDbContext(services, connectionStrings.Chat ?? connectionStrings.Default);
        AddContactDbContext(services, connectionStrings.Contact ?? connectionStrings.Default);
        AddStorageDbContext(services, connectionStrings.Storage ?? connectionStrings.Default);
        AddNotificationDbContext(services, connectionStrings.Notification ?? connectionStrings.Default);
        AddMessageDbContext(services, connectionStrings.Message ?? connectionStrings.Default);
        services.AddScoped<IDbConnectionFactory, NpgsqlDbConnectionFactory>();
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
        services.AddScoped<IChatRepository, ChatRepository>();
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
}