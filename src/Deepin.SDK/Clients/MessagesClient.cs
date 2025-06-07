using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Deepin.SDK.Configuration;
using Deepin.SDK.Models;

namespace Deepin.SDK.Clients;

/// <summary>
/// Client for message-related API endpoints
/// </summary>
public interface IMessagesClient
{
    Task<Message?> GetMessageAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Message>> GetMessagesAsync(GetMessagesRequest request, CancellationToken cancellationToken = default);
    Task<List<Message>> SearchMessagesAsync(SearchMessagesRequest request, CancellationToken cancellationToken = default);
    Task<Message?> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default);
    Task<List<Message>> GetLastMessagesAsync(GetLastMessagesRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of the messages client
/// </summary>
public class MessagesClient : BaseClient, IMessagesClient
{
    public MessagesClient(HttpClient httpClient, IOptions<DeepinApiOptions> options, ILogger<MessagesClient> logger)
        : base(httpClient, options, logger)
    {
    }

    public async Task<Message?> GetMessageAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<Message>($"api/messages/{id}", cancellationToken);
    }

    public async Task<List<Message>> GetMessagesAsync(GetMessagesRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<List<Message>>("api/messages", request, cancellationToken) ?? new List<Message>();
    }

    public async Task<List<Message>> SearchMessagesAsync(SearchMessagesRequest request, CancellationToken cancellationToken = default)
    {
        var queryParams = BuildQueryString(new Dictionary<string, object?>
        {
            ["query"] = request.Query,
            ["chatId"] = request.ChatId,
            ["skip"] = request.Skip,
            ["take"] = request.Take
        });

        return await GetAsync<List<Message>>($"api/messages/search{queryParams}", cancellationToken) ?? new List<Message>();
    }

    public async Task<Message?> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<Message>("api/messages", request, cancellationToken);
    }

    public async Task<List<Message>> GetLastMessagesAsync(GetLastMessagesRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<List<Message>>("api/messages/lasts", request, cancellationToken) ?? new List<Message>();
    }
}