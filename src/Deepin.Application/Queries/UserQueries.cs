using Dapper;
using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Users;
using Deepin.Application.Interfaces;

namespace Deepin.Application.Queries;

public interface IUserQueries
{
    Task<UserDto?> GetUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> GetUsersAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<IPagedResult<UserDto>> SearchUsersAsync(
        int limit,
        int offset,
        string? search = null,
        CancellationToken cancellationToken = default);
}

public class UserQueries(IDbConnectionFactory dbConnectionFactory) : IUserQueries
{
    public async Task<UserDto?> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rows = await GetUsersAsync([id], cancellationToken);
        if (rows == null || !rows.Any())
        {
            return null;
        }
        return rows.FirstOrDefault();
    }
    public async Task<IEnumerable<UserDto>> GetUsersAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateIdentityDbConnectionAsync(cancellationToken))
        {
            var sql = @"
                SELECT 
                    u.""Id"",
                    u.""UserName"",
                    u.""Email"",
                    u.""PhoneNumber"",
                    u.""TwoFactorEnabled"",
                    u.""EmailConfirmed"",
                    u.""PhoneNumberConfirmed"",
                    u.""CreatedAt"",
                    u.""UpdatedAt""
                FROM 
                    users AS u
                WHERE 
                    u.""Id"" = ANY(@ids)";
            var command = new CommandDefinition(sql, new { ids }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            if (rows == null || !rows.Any())
            {
                return Enumerable.Empty<UserDto>();
            }
            var userIds = rows.Select(u => (Guid)u.Id).Distinct().ToArray();
            var claimsRows = await GetUserClaimsAsync(userIds, cancellationToken);
            return rows.Select(u => MapUser(u, claimsRows.Where(c => c.UserId == u.Id)))
                       .Cast<UserDto>()
                       .ToList();
        }
    }
    public async Task<IPagedResult<UserDto>> SearchUsersAsync(
        int limit,
        int offset,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateIdentityDbConnectionAsync(cancellationToken))
        {
            var query = @"                
                SELECT 
                    u.""Id"",
                    u.""UserName"",
                    u.""Email"",
                    u.""PhoneNumber"",
                    u.""TwoFactorEnabled"",
                    u.""EmailConfirmed"",
                    u.""PhoneNumberConfirmed"",
                    u.""CreatedAt"",
                    u.""UpdatedAt""
                FROM 
                    users AS u";
            var countQuery = @"
                SELECT 
                    COUNT(u.""Id"") 
                FROM 
                    users u";
            var condations = new List<string>
            {
                @"1 = 1"
            };
            if (!string.IsNullOrWhiteSpace(search))
            {
                condations.Add(@"(u.""UserName"" ILIKE @search OR u.""Email"" ILIKE @search OR u.""PhoneNumber"" ILIKE @search)");
            }
            query += " WHERE " + string.Join(" AND ", condations);
            query += @" ORDER BY u.""CreatedAt"" DESC LIMIT @limit OFFSET @offset";
            countQuery += " WHERE " + string.Join(" AND ", condations);

            var countCommand = new CommandDefinition(countQuery, new { search = $"%{search}%" }, cancellationToken: cancellationToken);
            var totalCount = await connection.ExecuteScalarAsync<int>(countCommand);
            if (totalCount == 0)
            {
                return new PagedResult<UserDto>(new List<UserDto>(), 0, limit, offset);
            }
            var userQueryCommand = new CommandDefinition(query, new { search = $"%{search}%", limit, offset }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(userQueryCommand);
            var userIds = rows.Select(u => (Guid)u.Id).ToArray();
            var claimsRows = await GetUserClaimsAsync(userIds, cancellationToken);
            var users = rows
                .Select(u => MapUser(u, claimsRows.Where(c => c.UserId == u.Id)))
                .Cast<UserDto>()
                .ToList();

            return new PagedResult<UserDto>(users, offset, limit, totalCount);
        }
    }
    private async Task<IEnumerable<dynamic>> GetUserClaimsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        if (userIds == null || !userIds.Any())
        {
            return Enumerable.Empty<dynamic>();
        }
        using var connection = await dbConnectionFactory.CreateIdentityDbConnectionAsync(cancellationToken);
        var sql = @"
                SELECT 
                    uc.""Id"",
                    uc.""UserId"",
                    uc.""ClaimType"",
                    uc.""ClaimValue""
                FROM 
                    user_claims AS uc
                WHERE 
                    uc.""UserId"" = ANY(@userIds)";
        var command = new CommandDefinition(sql, new { userIds }, cancellationToken: cancellationToken);
        return await connection.QueryAsync<dynamic>(command);
    }
    private UserDto MapUser(dynamic row, IEnumerable<dynamic> claimsByUser)
    {
        var user = new UserDto
        {
            Id = row.Id,
            UserName = row.UserName,
            Email = row.Email,
            PhoneNumber = row.PhoneNumber,
            TwoFactorEnabled = row.TwoFactorEnabled,
            EmailConfirmed = row.EmailConfirmed,
            PhoneNumberConfirmed = row.PhoneNumberConfirmed,
            CreatedAt = row.CreatedAt,
            UpdatedAt = row.UpdatedAt
        };
        if (claimsByUser != null && claimsByUser.Any())
        {
            user.Claims = claimsByUser
                .Select(c => new UserCliamDto
                {
                    Id = c.Id,
                    ClaimType = c.ClaimType,
                    ClaimValue = c.ClaimValue
                })
                .ToList();
        }
        return user;
    }
}