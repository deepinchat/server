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
    Task<List<Message>> GetLastMessagesAsync(GetLastMessagesRequest request, CancellationToken cancellationToken = default);
    Task<Message?> GetMessageAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Message>> GetMessagesByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default);
    Task<ChatMessageUnreadCount?> GetUnreadCountAsync(GetUnreadMessageCountRequest request, CancellationToken cancellationToken = default);
    Task<List<Message>> SearchMessagesAsync(SearchMessagesRequest request, CancellationToken cancellationToken = default);
    Task<Message?> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default);
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

    public async Task<Message?> GetMessageAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<Message>($"api/v1/messages/{id}", cancellationToken);
    }

    public async Task<List<Message>> GetMessagesByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        return await PostAsync<List<Message>>("api/v1/messages", ids, cancellationToken) ?? new List<Message>();
    }

    public async Task<List<Message>> SearchMessagesAsync(SearchMessagesRequest request, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, object?>
        {
            ["offset"] = request.Offset,
            ["limit"] = request.Limit
        };
        if (!string.IsNullOrEmpty(request.Query))
        {
            query["search"] = request.Query;
        }
        if (request.ChatId.HasValue)
        {
            query["chatId"] = request.ChatId.Value;
        }
        if (request.UserId.HasValue)
        {
            query["userId"] = request.UserId.Value;
        }
        var queryParams = BuildQueryString(query);

        return await GetAsync<List<Message>>($"api/v1/messages/search{queryParams}", cancellationToken) ?? new List<Message>();
    }

    public async Task<Message?> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<Message>("api/v1/messages", request, cancellationToken);
    }

    public async Task<List<Message>> GetLastMessagesAsync(GetLastMessagesRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<List<Message>>("api/v1/messages/lasts", request, cancellationToken) ?? new List<Message>();
    }

    public async Task<ChatMessageUnreadCount?> GetUnreadCountAsync(GetUnreadMessageCountRequest request, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, object?>
        {
            ["chatId"] = request.ChatId,
            ["lastReadAt"] = request.LastReadAt?.ToString("o") // ISO 8601 format
        };
        var queryParams = BuildQueryString(query);

        return await GetAsync<ChatMessageUnreadCount>($"api/v1/messages/unread-count{queryParams}", cancellationToken);
    }
}