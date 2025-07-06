using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Deepin.Internal.SDK.Configuration;
using Deepin.Internal.SDK.Models;

namespace Deepin.Internal.SDK.Clients;

/// <summary>
/// Client for message-related API endpoints
/// </summary>
public interface IMessagesClient
{
    Task<List<LastMessageDto>> GetLastMessagesAsync(GetLastMessagesRequest request, CancellationToken cancellationToken = default);
    Task<MessageDto?> GetMessageAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<MessageDto>> BatchGetMessagesAsync(BatchGetMessageRequest request, CancellationToken cancellationToken = default);
    Task<IPagedResult<MessageDto>> GetPagedMessagesAsync(SearchMessagesRequest request, CancellationToken cancellationToken = default);
    Task<MessageDto?> SendMessageAsync(MessageRequest request, CancellationToken cancellationToken = default);
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

    public async Task<MessageDto?> GetMessageAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<MessageDto>($"api/v1/messages/{id}", cancellationToken);
    }

    public async Task<List<MessageDto>> BatchGetMessagesAsync(BatchGetMessageRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<List<MessageDto>>("api/v1/messages/batch", request, cancellationToken) ?? new List<MessageDto>();
    }

    public async Task<IPagedResult<MessageDto>> GetPagedMessagesAsync(SearchMessagesRequest request, CancellationToken cancellationToken = default)
    {
        var sortBy = request.SortBy ?? SortDirection.Descending;
        var query = new Dictionary<string, object?>
        {
            ["offset"] = request.Offset,
            ["limit"] = request.Limit,
            ["sortBy"] = sortBy.ToString().ToLowerInvariant()
        };
        if (!string.IsNullOrEmpty(request.Search))
        {
            query["search"] = request.Search;
        }
        if (request.ChatId.HasValue)
        {
            query["chatId"] = request.ChatId.Value;
        }
        if (request.UserId.HasValue)
        {
            query["userId"] = request.UserId.Value;
        }
        if (request.ReadAt.HasValue)
        {
            query["readAt"] = request.ReadAt.Value;
        }
        var queryParams = BuildQueryString(query);

        return await GetAsync<PagedResult<MessageDto>>($"api/v1/messages{queryParams}", cancellationToken)
            ?? new PagedResult<MessageDto>(new List<MessageDto>(), 0, request.Offset, request.Limit);
    }

    public async Task<MessageDto?> SendMessageAsync(MessageRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<MessageDto>("api/v1/messages", request, cancellationToken);
    }

    public async Task<List<LastMessageDto>> GetLastMessagesAsync(GetLastMessagesRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<List<LastMessageDto>>("api/v1/messages/lasts", request, cancellationToken) ?? new List<LastMessageDto>();
    }
}