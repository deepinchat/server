using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Deepin.SDK.Configuration;
using Deepin.SDK.Models;

namespace Deepin.SDK.Clients;

/// <summary>
/// Client for chat-related API endpoints
/// </summary>
public interface IChatsClient
{
    Task<Chat?> CreateChatAsync(CreateChatRequest request, CancellationToken cancellationToken = default);
    Task<Chat?> CreateDirectChatAsync(CreateDirectChatRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteChatAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Chat?> GetChatAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IPagedResult<ChatMember>> GetChatMembersAsync(Guid id, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);
    Task<List<Chat>> GetChatsAsync(CancellationToken cancellationToken = default);
    Task<ReadStatus?> GetReadStatusAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ReadStatus>> GetReadStatusesAsync(CancellationToken cancellationToken = default);
    Task<bool> JoinChatAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> LeaveChatAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IPagedResult<Chat>> SearchChatsAsync(SearchChatRequest request, CancellationToken cancellationToken = default);
    Task<Chat?> UpdateChatAsync(Guid id, UpdateChatRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateReadStatusAsync(Guid id, UpdateReadStatusRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of the chats client
/// </summary>
public class ChatsClient : BaseClient, IChatsClient
{
    public ChatsClient(HttpClient httpClient, IOptions<DeepinApiOptions> options, ILogger<ChatsClient> logger)
        : base(httpClient, options, logger)
    {
    }

    public async Task<Chat?> GetChatAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<Chat>($"api/v1/chats/{id}", cancellationToken);
    }

    public async Task<List<Chat>> GetChatsAsync(CancellationToken cancellationToken = default)
    {
        return await GetAsync<List<Chat>>("api/v1/chats", cancellationToken) ?? new List<Chat>();
    }

    public async Task<IPagedResult<Chat>> SearchChatsAsync(SearchChatRequest request, CancellationToken cancellationToken = default)
    {
        var queryParams = BuildQueryString(new Dictionary<string, object?>
        {
            ["search"] = request.Search,
            ["type"] = request.Type?.ToString(),
            ["offset"] = request.Offset,
            ["limit"] = request.Limit
        });

        return await GetAsync<IPagedResult<Chat>>($"api/v1/chats/search{queryParams}", cancellationToken) ?? new PagedResult<Chat>();
    }

    public async Task<Chat?> CreateChatAsync(CreateChatRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<Chat>("api/v1/chats", request, cancellationToken);
    }

    public async Task<Chat?> UpdateChatAsync(Guid id, UpdateChatRequest request, CancellationToken cancellationToken = default)
    {
        return await PutAsync<Chat>($"api/v1/chats/{id}", request, cancellationToken);
    }

    public async Task<Chat?> CreateDirectChatAsync(CreateDirectChatRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<Chat>("api/v1/chats/direct", request.UserIds, cancellationToken);
    }

    public async Task<bool> DeleteChatAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await DeleteAsync<object>($"api/v1/chats/{id}", cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> JoinChatAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await PostAsync<object>($"api/v1/chats/{id}/join", null, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> LeaveChatAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await PostAsync<object>($"api/v1/chats/{id}/leave", null, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IPagedResult<ChatMember>> GetChatMembersAsync(Guid id, int offset = 0, int limit = 20, CancellationToken cancellationToken = default)
    {
        return await GetAsync<IPagedResult<ChatMember>>($"api/v1/chats/{id}/members?offset={offset}&limit={limit}", cancellationToken) ?? new PagedResult<ChatMember>();
    }

    public async Task<List<ReadStatus>> GetReadStatusesAsync(CancellationToken cancellationToken = default)
    {
        return await GetAsync<List<ReadStatus>>("api/v1/chats/read-statuses", cancellationToken) ?? new List<ReadStatus>();
    }

    public async Task<ReadStatus?> GetReadStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<ReadStatus>($"api/v1/chats/{id}/read-status", cancellationToken);
    }

    public async Task<bool> UpdateReadStatusAsync(Guid id, UpdateReadStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await PostAsync<object>($"api/v1/chats/{id}/read-status", request, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}