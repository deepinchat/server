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
                    u.""UpdatedAt"",
                    uc.""Id"" AS ""ClaimId"",
                    uc.""ClaimType"",
                    uc.""ClaimValue""
                FROM 
                    users AS u
                LEFT JOIN
                    user_claims AS uc ON u.""Id"" = uc.""UserId""
                WHERE 
                    u.""Id"" = ANY(@ids)";
            var command = new CommandDefinition(sql, new { ids }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            return rows
                .GroupBy(r => r.Id)
                .Select(g => MapUser(g.ToArray()))
                .Where(u => u != null)
                .Select(u => u!);
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
                    u.""UpdatedAt"",
                    uc.""Id"" AS ""ClaimId"",
                    uc.""ClaimType"",
                    uc.""ClaimValue""
                FROM 
                    users AS u
                LEFT JOIN
                    user_claims AS uc ON u.""Id"" = uc.""UserId""";
            var countQuery = @"
                SELECT COUNT(DISTINCT u.""Id"") 
                FROM 
                    users u
                LEFT JOIN
                    user_claims AS uc ON u.""Id"" = uc.""UserId""";
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
            var command = new CommandDefinition(query, new { search = $"%{search}%", limit, offset }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            var users = rows
                .GroupBy(r => r.Id)
                .Select(g => MapUser(g.ToArray()))
                .Where(u => u != null)
                .Select(u => u!)
                .ToList();
            return new PagedResult<UserDto>(users, totalCount, limit, offset);
        }
    }
    private UserDto? MapUser(dynamic[] rows)
    {
        var firstRow = rows.FirstOrDefault();
        if (firstRow == null)
        {
            return null;
        }
        var user = new UserDto
        {
            Id = firstRow.Id,
            UserName = firstRow.UserName,
            Email = firstRow.Email,
            PhoneNumber = firstRow.PhoneNumber,
            TwoFactorEnabled = firstRow.TwoFactorEnabled,
            EmailConfirmed = firstRow.EmailConfirmed,
            PhoneNumberConfirmed = firstRow.PhoneNumberConfirmed,
            CreatedAt = firstRow.CreatedAt,
            UpdatedAt = firstRow.UpdatedAt
        };
        var userCliams = new List<UserCliamDto>();
        foreach (var row in rows)
        {
            if (row.ClaimId != null && row.ClaimId > 0)
            {
                userCliams.Add(new UserCliamDto
                {
                    Id = row.ClaimId,
                    ClaimType = row.ClaimType,
                    ClaimValue = row.ClaimValue
                });
            }
        }
        return user;
    }
}