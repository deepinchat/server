using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Deepin.Internal.SDK.Configuration;

namespace Deepin.Internal.SDK.Clients;

/// <summary>
/// Base class for all API clients
/// </summary>
public abstract class BaseClient
{
    protected readonly HttpClient HttpClient;
    protected readonly DeepinApiOptions Options;
    protected readonly ILogger Logger;
    protected readonly JsonSerializerOptions JsonOptions;

    protected BaseClient(HttpClient httpClient, IOptions<DeepinApiOptions> options, ILogger logger)
    {
        HttpClient = httpClient;
        Options = options.Value;
        Logger = logger;

        JsonOptions = options.Value.JsonSerializerOptions ?? new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };
    }


    /// <summary>
    /// Sends a GET request and returns the response
    /// </summary>
    protected async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogDebug("Sending GET request to {Endpoint}", endpoint);
            var response = await HttpClient.GetAsync(endpoint, cancellationToken);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while sending GET request to {Endpoint}", endpoint);
            throw;
        }
    }
    protected async Task<HttpResponseMessage> PostAsync(string endpoint, object? content = null, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogDebug("Sending POST request to {Endpoint}", endpoint);
            var response = await HttpClient.PostAsJsonAsync(endpoint, content, JsonOptions, cancellationToken);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while sending POST request to {Endpoint}", endpoint);
            throw;
        }
    }
    /// <summary>
    /// Sends a POST request and returns the response
    /// </summary>
    protected async Task<T?> PostAsync<T>(string endpoint, object? content = null, CancellationToken cancellationToken = default)
    {
        var response = await this.PostAsync(endpoint, content, cancellationToken);
        return await HandleResponse<T>(response);
    }
    protected async Task<HttpResponseMessage> PutAsync(string endpoint, object? content = null, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogDebug("Sending PUT request to {Endpoint}", endpoint);
            var response = await HttpClient.PutAsJsonAsync(endpoint, content, JsonOptions, cancellationToken);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while sending PUT request to {Endpoint}", endpoint);
            throw;
        }
    }
    /// <summary>
    /// Sends a PUT request and returns the response
    /// </summary>
    protected async Task<T?> PutAsync<T>(string endpoint, object? content = null, CancellationToken cancellationToken = default)
    {
        var response = await this.PutAsync(endpoint, content, cancellationToken);
        return await HandleResponse<T>(response);
    }

    /// <summary>
    /// Sends a DELETE request and returns the response
    /// </summary>
    protected async Task<HttpResponseMessage> DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Sending DELETE request to {Endpoint}", endpoint);
        var response = await HttpClient.DeleteAsync(endpoint, cancellationToken);
        response.EnsureSuccessStatusCode();
        return response;
    }

    /// <summary>
    /// Handles the HTTP response and deserializes the content
    /// </summary>
    private async Task<T?> HandleResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Logger.LogWarning("Request failed with status {StatusCode}: {Content}",
                response.StatusCode, content);

            var errorMessage = $"Request failed with status {response.StatusCode}";
            if (!string.IsNullOrEmpty(content))
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(content, JsonOptions);
                    if (errorResponse?.ContainsKey("message") == true)
                    {
                        errorMessage = errorResponse["message"].ToString() ?? errorMessage;
                    }
                }
                catch
                {
                    // Ignore JSON parsing errors
                }
            }
            throw new HttpRequestException(errorMessage);
        }

        if (typeof(T) == typeof(string))
        {
            return (T)(object)content;
        }

        if (string.IsNullOrEmpty(content))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(content, JsonOptions);
    }

    /// <summary>
    /// Builds query string from parameters
    /// </summary>
    protected string BuildQueryString(Dictionary<string, object?> parameters)
    {
        var queryParams = parameters
            .Where(p => p.Value != null)
            .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value!.ToString() ?? string.Empty)}")
            .ToArray();

        return queryParams.Length > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
    }
}