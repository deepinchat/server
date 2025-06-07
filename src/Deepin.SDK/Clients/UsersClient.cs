using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Deepin.SDK.Configuration;
using Deepin.SDK.Models;

namespace Deepin.SDK.Clients;

/// <summary>
/// Client for user-related API endpoints
/// </summary>
public interface IUsersClient
{
    Task<User?> GetUserAsync(int id, CancellationToken cancellationToken = default);
    Task<List<User>> GetUsersByIdsAsync(GetUsersByIdsRequest request, CancellationToken cancellationToken = default);
    Task<List<User>> GetUsersByIdsAsync(List<int> userIds, CancellationToken cancellationToken = default);
    Task<List<User>> SearchUsersAsync(SearchUsersRequest request, CancellationToken cancellationToken = default);
    Task<List<User>> SearchUsersAsync(string query, int skip = 0, int take = 20, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of the users client
/// </summary>
public class UsersClient : BaseClient, IUsersClient
{
    public UsersClient(HttpClient httpClient, IOptions<DeepinApiOptions> options, ILogger<UsersClient> logger)
        : base(httpClient, options, logger)
    {
    }

    public async Task<User?> GetUserAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<User>($"api/user/{id}", cancellationToken);
    }

    public async Task<List<User>> GetUsersByIdsAsync(GetUsersByIdsRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<List<User>>("api/user/batch", request, cancellationToken) ?? new List<User>();
    }

    public async Task<List<User>> GetUsersByIdsAsync(List<int> userIds, CancellationToken cancellationToken = default)
    {
        var request = new GetUsersByIdsRequest { UserIds = userIds };
        return await GetUsersByIdsAsync(request, cancellationToken);
    }

    public async Task<List<User>> SearchUsersAsync(SearchUsersRequest request, CancellationToken cancellationToken = default)
    {
        var queryParams = BuildQueryString(new Dictionary<string, object?>
        {
            ["query"] = request.Query,
            ["skip"] = request.Skip,
            ["take"] = request.Take
        });

        return await GetAsync<List<User>>($"api/user/search{queryParams}", cancellationToken) ?? new List<User>();
    }

    public async Task<List<User>> SearchUsersAsync(string query, int skip = 0, int take = 20, CancellationToken cancellationToken = default)
    {
        var request = new SearchUsersRequest 
        { 
            Query = query, 
            Skip = skip, 
            Take = take 
        };
        return await SearchUsersAsync(request, cancellationToken);
    }
}