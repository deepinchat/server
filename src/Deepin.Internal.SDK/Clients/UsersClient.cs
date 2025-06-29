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
    Task<UserDto?> GetUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<UserDto>> BatchGetUsersAsync(BatchGetUsersRequest request, CancellationToken cancellationToken = default);
    Task<IPagedResult<UserDto>> SearchUsersAsync(SearchUsersRequest request, CancellationToken cancellationToken = default);
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

    public async Task<UserDto?> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<UserDto>($"api/v1/users/{id}", cancellationToken);
    }

    public async Task<List<UserDto>> BatchGetUsersAsync(BatchGetUsersRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<List<UserDto>>("api/v1/users/batch", request, cancellationToken) ?? new List<UserDto>();
    }

    public async Task<IPagedResult<UserDto>> SearchUsersAsync(SearchUsersRequest request, CancellationToken cancellationToken = default)
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

        var response = await GetAsync<IPagedResult<UserDto>>($"api/v1/users/search{queryParams}", cancellationToken);

        return response ?? new PagedResult<UserDto>(new List<UserDto>(), 0, request.Offset, request.Limit);
    }
}