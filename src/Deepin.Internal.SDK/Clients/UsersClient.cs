using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Deepin.Internal.SDK.Configuration;
using Deepin.Internal.SDK.Models;

namespace Deepin.Internal.SDK.Clients;

/// <summary>
/// Client for user-related API endpoints
/// </summary>
public interface IUsersClient
{
    Task<User?> GetUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<User>> GetUsersByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default);
    Task<List<User>> SearchUsersAsync(SearchUsersRequest request, CancellationToken cancellationToken = default);
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

    public async Task<User?> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<User>($"api/v1/user/{id}", cancellationToken);
    }

    public async Task<List<User>> GetUsersByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        return await PostAsync<List<User>>("api/v1/user/batch", ids, cancellationToken) ?? new List<User>();
    }

    public async Task<List<User>> SearchUsersAsync(SearchUsersRequest request, CancellationToken cancellationToken = default)
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
        var queryParams = BuildQueryString(query);

        return await GetAsync<List<User>>($"api/v1/user/search{queryParams}", cancellationToken) ?? new List<User>();
    }
}