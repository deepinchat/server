using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Deepin.Internal.SDK.Configuration;
using Deepin.Internal.SDK.Clients;

namespace Deepin.Internal.SDK.Extensions;

/// <summary>
/// Extension methods for registering Deepin SDK services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Deepin API client services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configure">Configuration action for Deepin API options</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection AddDeepinApiClient(
        this IServiceCollection services,
        Action<DeepinApiOptions> configure)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configure == null) throw new ArgumentNullException(nameof(configure));

        // Configure options
        services.Configure(configure);

        // Add HTTP client
        services.AddHttpClient<IDeepinApiClient, DeepinApiClient>((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<DeepinApiOptions>>().Value;
            
            if (!string.IsNullOrEmpty(options.BaseUrl))
            {
                httpClient.BaseAddress = new Uri(options.BaseUrl);
            }

            httpClient.Timeout = options.Timeout;

            if (!string.IsNullOrEmpty(options.AccessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.AccessToken);
            }

            foreach (var header in options.DefaultHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        });

        // Register individual clients
        services.AddTransient<IChatsClient>(serviceProvider =>
        {
            var httpClient = serviceProvider.GetRequiredService<HttpClient>();
            var options = serviceProvider.GetRequiredService<IOptionsMonitor<DeepinApiOptions>>();
            var logger = serviceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<ChatsClient>>();
            var optionsWrapper = new OptionsWrapper<DeepinApiOptions>(options.CurrentValue);
            return new ChatsClient(httpClient, optionsWrapper, logger);
        });

        services.AddTransient<IMessagesClient>(serviceProvider =>
        {
            var httpClient = serviceProvider.GetRequiredService<HttpClient>();
            var options = serviceProvider.GetRequiredService<IOptionsMonitor<DeepinApiOptions>>();
            var logger = serviceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<MessagesClient>>();
            var optionsWrapper = new OptionsWrapper<DeepinApiOptions>(options.CurrentValue);
            return new MessagesClient(httpClient, optionsWrapper, logger);
        });

        services.AddTransient<IFilesClient>(serviceProvider =>
        {
            var httpClient = serviceProvider.GetRequiredService<HttpClient>();
            var options = serviceProvider.GetRequiredService<IOptionsMonitor<DeepinApiOptions>>();
            var logger = serviceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<FilesClient>>();
            var optionsWrapper = new OptionsWrapper<DeepinApiOptions>(options.CurrentValue);
            return new FilesClient(httpClient, optionsWrapper, logger);
        });

        services.AddTransient<IUsersClient>(serviceProvider =>
        {
            var httpClient = serviceProvider.GetRequiredService<HttpClient>();
            var options = serviceProvider.GetRequiredService<IOptionsMonitor<DeepinApiOptions>>();
            var logger = serviceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<UsersClient>>();
            var optionsWrapper = new OptionsWrapper<DeepinApiOptions>(options.CurrentValue);
            return new UsersClient(httpClient, optionsWrapper, logger);
        });

        return services;
    }

    /// <summary>
    /// Adds Deepin API client services to the service collection with configuration from appsettings
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration instance</param>
    /// <param name="sectionName">The configuration section name (default: "DeepinApi")</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection AddDeepinApiClient(
        this IServiceCollection services,
        Microsoft.Extensions.Configuration.IConfiguration configuration,
        string sectionName = "DeepinApi")
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return services.AddDeepinApiClient(options =>
        {
            configuration.GetSection(sectionName).Bind(options);
        });
    }

    /// <summary>
    /// Adds Deepin API client services with a base URL
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="baseUrl">The base URL of the Deepin API</param>
    /// <param name="accessToken">Optional access token for authentication</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection AddDeepinApiClient(
        this IServiceCollection services,
        string baseUrl,
        string? accessToken = null)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (string.IsNullOrEmpty(baseUrl)) throw new ArgumentException("Base URL cannot be null or empty", nameof(baseUrl));

        return services.AddDeepinApiClient(options =>
        {
            options.BaseUrl = baseUrl;
            if (!string.IsNullOrEmpty(accessToken))
            {
                options.AccessToken = accessToken;
            }
        });
    }
}
