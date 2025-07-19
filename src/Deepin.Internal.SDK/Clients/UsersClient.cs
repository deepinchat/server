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
    Task<UserDto?> UpdateUserClaimsAsync(Guid id, IEnumerable<UserCliamRequest> requests, CancellationToken cancellationToken = default);
    Task<List<UserDto>> BatchGetUsersAsync(BatchGetUsersRequest request, CancellationToken cancellationToken = default);
    Task<IPagedResult<UserDto>> SearchUsersAsync(SearchUsersRequest request, CancellationToken cancellationToken = default);
    Task<UserDto?> GetUserByIdentityAsync(string identity, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of the users client
/// </summary>
public class UsersClient : BaseClient, IUsersClient
{
    public UsersClient(HttpClient httpClient, IOptions<DeepinApiOptions> options, ILogger logger)
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
        var sortBy = request.SortBy ?? SortDirection.Desc;
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
        var queryParams = BuildQueryString(query);

        var response = await GetAsync<PagedResult<UserDto>>($"api/v1/users/search{queryParams}", cancellationToken);

        return response ?? new PagedResult<UserDto>(new List<UserDto>(), 0, request.Offset, request.Limit);
    }

    public async Task<UserDto?> UpdateUserClaimsAsync(Guid id, IEnumerable<UserCliamRequest> requests, CancellationToken cancellationToken = default)
    {
        if (requests == null || !requests.Any())
        {
            throw new ArgumentException("Claims cannot be null or empty.", nameof(requests));
        }

        return await PostAsync<UserDto>($"api/v1/users/{id}/claims", requests, cancellationToken);
    }

    public async Task<UserDto?> GetUserByIdentityAsync(string identity, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(identity))
        {
            throw new ArgumentException("Identity cannot be null or empty.", nameof(identity));
        }

        return await GetAsync<UserDto>($"api/v1/users/identity/{Uri.EscapeDataString(identity)}", cancellationToken);
    }
}