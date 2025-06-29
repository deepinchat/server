using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Deepin.Internal.SDK.Configuration;
using Deepin.Internal.SDK.Models;

namespace Deepin.Internal.SDK.Clients;

/// <summary>
/// Client for chat-related API endpoints
/// </summary>
public interface IChatsClient
{
    Task<GroupChatDto?> CreateGroupChatAsync(CreateGroupChatRequest request, CancellationToken cancellationToken = default);
    Task<GroupChatDto?> UpdateGroupChatAsync(Guid id, UpdateGroupChatRequest request, CancellationToken cancellationToken = default);
    Task<DirectChatDto?> CreateDirectChatAsync(Guid targetUserId, CancellationToken cancellationToken = default);
    Task<bool> DeleteChatAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GroupChatDto?> GetGroupChatAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DirectChatDto?> GetDirectChatAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IPagedResult<ChatMemberDto>> GetChatMembersAsync(Guid id, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupChatDto>> GetGroupChatsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DirectChatDto>> GetDirectChatsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ChatUnreadCountDto>> GetUnreadCounts(CancellationToken cancellationToken = default);
    Task<bool> JoinChatAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> LeaveChatAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IPagedResult<GroupChatDto>> SearchChatsAsync(SearchChatRequest request, CancellationToken cancellationToken = default);
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

    public async Task<DirectChatDto?> CreateDirectChatAsync(Guid targetUserId, CancellationToken cancellationToken = default)
    {
        return await PostAsync<DirectChatDto?>($"api/v1/chats/direct", new
        {
            TargetUserId = targetUserId
        }, cancellationToken);
    }

    public async Task<GroupChatDto?> CreateGroupChatAsync(CreateGroupChatRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<GroupChatDto?>($"api/v1/chats/group", request, cancellationToken);
    }

    public async Task<bool> DeleteChatAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"api/v1/chats/{id}", cancellationToken);
        return true;
    }

    public async Task<IPagedResult<ChatMemberDto>> GetChatMembersAsync(Guid id, int offset = 0, int limit = 20, CancellationToken cancellationToken = default)
    {
        var response = await GetAsync<IPagedResult<ChatMemberDto>>($"api/v1/chats/{id}/members?offset={offset}&limit={limit}", cancellationToken);
        return response ?? new PagedResult<ChatMemberDto>(Array.Empty<ChatMemberDto>(), 0, offset, limit);
    }

    public async Task<DirectChatDto?> GetDirectChatAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await GetAsync<DirectChatDto?>($"api/v1/chats/direct/{id}", cancellationToken);
        return response;
    }

    public async Task<IEnumerable<DirectChatDto>> GetDirectChatsAsync(CancellationToken cancellationToken = default)
    {
        var response = await GetAsync<IEnumerable<DirectChatDto>>("api/v1/chats/direct", cancellationToken);
        return response ?? Array.Empty<DirectChatDto>();
    }

    public async Task<GroupChatDto?> GetGroupChatAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await GetAsync<GroupChatDto?>($"api/v1/chats/group/{id}", cancellationToken);
        return response;
    }

    public async Task<IEnumerable<GroupChatDto>> GetGroupChatsAsync(CancellationToken cancellationToken = default)
    {
        var response = await GetAsync<IEnumerable<GroupChatDto>>("api/v1/chats/group", cancellationToken);
        return response ?? Array.Empty<GroupChatDto>();
    }

    public async Task<IEnumerable<ChatUnreadCountDto>> GetUnreadCounts(CancellationToken cancellationToken = default)
    {
        var response = await GetAsync<IEnumerable<ChatUnreadCountDto>>("api/v1/chats/unread-counts", cancellationToken);
        return response ?? Array.Empty<ChatUnreadCountDto>();
    }

    public async Task<bool> JoinChatAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await PostAsync($"api/v1/chats/{id}/join", null, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> LeaveChatAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await PostAsync($"api/v1/chats/{id}/leave", null, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<IPagedResult<GroupChatDto>> SearchChatsAsync(SearchChatRequest request, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, object?>
        {
            ["offset"] = request.Offset,
            ["limit"] = request.Limit,
            ["search"] = request.Search
        };
        var queryParams = BuildQueryString(query);
        var response = await GetAsync<IPagedResult<GroupChatDto>>($"api/v1/chats/search{queryParams}", cancellationToken);
        return response ?? new PagedResult<GroupChatDto>(Array.Empty<GroupChatDto>(), 0, request.Offset, request.Limit);
    }

    public async Task<GroupChatDto?> UpdateGroupChatAsync(Guid id, UpdateGroupChatRequest request, CancellationToken cancellationToken = default)
    {
        return await PutAsync<GroupChatDto?>($"api/v1/chats/group/{id}", request, cancellationToken);
    }
}