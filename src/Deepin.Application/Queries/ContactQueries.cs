using Dapper;
using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Contacts;
using Deepin.Application.Interfaces;

namespace Deepin.Application.Queries;

public interface IContactQueries
{
    Task<ContactDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IPagedResult<ContactDto>> GetPagedAsync(int offset, int limit, string? search = null, CancellationToken cancellationToken = default);
}

public class ContactQueries(IDbConnectionFactory dbConnectionFactory) : IContactQueries
{
    public async Task<ContactDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = await dbConnectionFactory.CreateContactDbConnectionAsync();
        var query =
        @"SELECT
                id,
                name,
                email,
                phone_number,
                created_at,
                updated_at,
                is_blocked,
                is_starred,
                address,
                notes,
                birthday,
                company,
                created_by,
                first_name,
                last_name,
                user_id
        FROM contacts WHERE id = @id AND is_deleted = false";
        var row = await connection.QuerySingleOrDefaultAsync<dynamic>(new CommandDefinition(query, new { id }, cancellationToken: cancellationToken));
        if (row == null)
        {
            return null;
        }
        return MapToContactDto(row);
    }
    public async Task<IPagedResult<ContactDto>> GetPagedAsync(int offset, int limit, string? search = null, CancellationToken cancellationToken = default)
    {
        var condations = new List<string>
            {
                @"is_deleted IS FALSE"
            };
        if (!string.IsNullOrWhiteSpace(search))
        {
            condations.Add(@"(name ILIKE @search OR email ILIKE @search OR phone_number ILIKE @search)");
        }
        var whereClause = string.Join(" AND ", condations);
        var query = $@"
            SELECT
                id,
                name,
                email,
                phone_number,
                created_at,
                updated_at,
                is_blocked,
                is_starred,
                address,
                notes,
                birthday,
                company,
                created_by,
                first_name,
                last_name,
                user_id
            FROM 
                contacts
            WHERE 
                {whereClause}
            ORDER BY 
                created_at DESC
            OFFSET @offset LIMIT @limit";
        var countQuery = $@"
            SELECT 
                COUNT(id) 
            FROM 
                contacts 
            WHERE 
                {whereClause}";
        using var connection = await dbConnectionFactory.CreateContactDbConnectionAsync();
        var countCommand = new CommandDefinition(countQuery, new { search = $"%{search}%" }, cancellationToken: cancellationToken);
        var totalCount = await connection.ExecuteScalarAsync<int>(countCommand);
        if (totalCount == 0)
        {
            return new PagedResult<ContactDto>([], offset, limit, 0);
        }
        var command = new CommandDefinition(query, new { search = $"%{search}%", offset, limit }, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<dynamic>(command);
        var contacts = rows.Select(row => MapToContactDto(row)).Cast<ContactDto>().ToList();
        return new PagedResult<ContactDto>(contacts, offset, limit, totalCount);
    }
    private ContactDto MapToContactDto(dynamic row)
    {
        return new ContactDto
        {
            Id = row.id,
            Name = row.name,
            Email = row.email,
            PhoneNumber = row.phone_number,
            CreatedAt = row.created_at,
            UpdatedAt = row.updated_at,
            IsBlocked = row.is_blocked,
            IsStarred = row.is_starred,
            Address = row.address,
            Notes = row.notes,
            Birthday = row.birthday,
            Company = row.company,
            CreatedBy = row.created_by,
            FirstName = row.first_name,
            LastName = row.last_name,
            UserId = row.user_id
        };
    }
}
