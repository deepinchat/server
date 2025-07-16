using Deepin.Internal.SDK.Configuration;
using Deepin.Internal.SDK.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Deepin.Internal.SDK.Clients;

public interface IContactApiClient
{
    Task<ContactDto?> CreateContactAsync(CreateContactRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteContactAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ContactDto?> GetContactAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IPagedResult<ContactDto>> SearchContactsAsync(SearchContactRequest request, CancellationToken cancellationToken = default);
    Task<ContactDto?> UpdateContactAsync(Guid id, UpdateContactRequest request, CancellationToken cancellationToken = default);
}

public class ContactApiClient(HttpClient httpClient, IOptions<DeepinApiOptions> options, ILogger logger)
        : BaseClient(httpClient, options, logger), IContactApiClient
{
    public async Task<ContactDto?> GetContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<ContactDto>($"api/v1/contacts/{id}", cancellationToken);
    }

    public async Task<ContactDto?> CreateContactAsync(CreateContactRequest request, CancellationToken cancellationToken = default)
    {
        return await PostAsync<ContactDto?>("api/v1/contacts", request, cancellationToken);
    }

    public async Task<ContactDto?> UpdateContactAsync(Guid id, UpdateContactRequest request, CancellationToken cancellationToken = default)
    {
        return await PutAsync<ContactDto?>($"api/v1/contacts/{id}", request, cancellationToken);
    }

    public async Task<bool> DeleteContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"api/v1/contacts/{id}", cancellationToken);
        return true;
    }

    public async Task<IPagedResult<ContactDto>> SearchContactsAsync(SearchContactRequest request, CancellationToken cancellationToken = default)
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

        var response = await GetAsync<PagedResult<ContactDto>>($"api/v1/contacts{queryParams}", cancellationToken);

        return response ?? new PagedResult<ContactDto>([], 0, request.Offset, request.Limit);
    }

}
