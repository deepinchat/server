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
    Task<Chat?> GetChatAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Chat>> GetChatsAsync(CancellationToken cancellationToken = default);
    Task<List<Chat>> SearchChatsAsync(string query, int skip = 0, int take = 20, CancellationToken cancellationToken = default);
    Task<Chat?> CreateChatAsync(CreateChatRequest request, CancellationToken cancellationToken = default);
    Task<Chat?> UpdateChatAsync(int id, UpdateChatRequest request, CancellationToken cancellationToken = default);
    Task<Chat?> CreateDirectChatAsync(CreateDirectChatRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteChatAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> JoinChatAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> LeaveChatAsync(int id, CancellationToken cancellationToken = default);
    Task<List<ChatMember>> GetChatMembersAsync(int id, CancellationToken cancellationToken = default);
    Task<List<ReadStatus>> GetReadStatusesAsync(CancellationToken cancellationToken = default);
    Task<ReadStatus?> GetReadStatusAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> UpdateReadStatusAsync(int id, UpdateReadStatusRequest request, CancellationToken cancellationToken = default);
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

    public async Task<Chat?> GetChatAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<Chat>($"api/chats/{id}", cancellationToken);
    }

    public async Task<List<Chat>> GetChatsAsync(CancellationToken cancellationToken = default)
    {
        return await GetAsync<List<Chat>>("api/chats", cancellationToken) ?? new List<Chat>();
    }

    public async Task<List<Chat>> SearchChatsAsync(string query, int skip = 0, int take = 20, CancellationToken cancellationToken = default)
    {
        var queryParams = BuildQueryString(new Dictionary<string, object?>
        {
            ["query"] = query,
            ["skip"] = skip,
            ["take"] = take
        });

        return await GetAsync<List<Chat>>($"api/chats/search{queryParams}", cancellationToken) ?? new List<Chat>();
    }

    public async Task<Chat?> CreateChatAsync(CreateChatRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<Chat>("api/chats", request, cancellationToken);
    }

    public async Task<Chat?> UpdateChatAsync(int id, UpdateChatRequest request, CancellationToken cancellationToken = default)
    {
        return await PutAsync<Chat>($"api/chats/{id}", request, cancellationToken);
    }

    public async Task<Chat?> CreateDirectChatAsync(CreateDirectChatRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<Chat>("api/chats/direct", request, cancellationToken);
    }

    public async Task<bool> DeleteChatAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await DeleteAsync<object>($"api/chats/{id}", cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> JoinChatAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await PostAsync<object>($"api/chats/{id}/join", null, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> LeaveChatAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await PostAsync<object>($"api/chats/{id}/leave", null, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<ChatMember>> GetChatMembersAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<List<ChatMember>>($"api/chats/{id}/members", cancellationToken) ?? new List<ChatMember>();
    }

    public async Task<List<ReadStatus>> GetReadStatusesAsync(CancellationToken cancellationToken = default)
    {
        return await GetAsync<List<ReadStatus>>("api/chats/read-statuses", cancellationToken) ?? new List<ReadStatus>();
    }

    public async Task<ReadStatus?> GetReadStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<ReadStatus>($"api/chats/{id}/read-status", cancellationToken);
    }

    public async Task<bool> UpdateReadStatusAsync(int id, UpdateReadStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            await PostAsync<object>($"api/chats/{id}/read-status", request, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}