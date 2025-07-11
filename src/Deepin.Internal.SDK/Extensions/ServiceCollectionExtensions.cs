using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Deepin.Internal.SDK.Configuration;
using Deepin.Internal.SDK.Clients;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    public static IHttpClientBuilder AddDeepinApiClient(this IServiceCollection services, Action<DeepinApiOptions>? configure = null)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configure == null)
        {
            configure = options =>
            {
                options.BaseUrl = "https://api.deepin.chat";
                options.Timeout = TimeSpan.FromSeconds(60);
                options.JsonSerializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = true
                };
            };
        }

        // Configure options
        services.Configure(configure);

        // Add HTTP client
        return services.AddHttpClient<IDeepinApiClient, DeepinApiClient>((serviceProvider, httpClient) =>
         {
             var options = serviceProvider.GetRequiredService<IOptions<DeepinApiOptions>>().Value;

             if (!string.IsNullOrEmpty(options.BaseUrl))
             {
                 httpClient.BaseAddress = new Uri(options.BaseUrl);
             }

             httpClient.Timeout = options.Timeout;

             foreach (var header in options.DefaultHeaders)
             {
                 httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
             }
         });
    }
}
