using System.Text.Json;

namespace Deepin.Internal.SDK.Configuration;

/// <summary>
/// Configuration options for the Deepin API client
/// </summary>
public class DeepinApiOptions
{
    /// <summary>
    /// The base URL of the Deepin API
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// The timeout for HTTP requests
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Additional headers to include with requests
    /// </summary>
    public Dictionary<string, string> DefaultHeaders { get; set; } = new();
    /// <summary>
    /// JSON serializer options for request and response serialization
    /// </summary>
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }
}
