using Dapper;
using Deepin.Application.DTOs.Users;
using Deepin.Application.Interfaces;

namespace Deepin.Application.Queries;

public interface IUserQueries
{
    Task<UserDto?> GetUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> GetUsersAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
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
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.PhoneNumber,
                    u.TwoFactorEnabled,
                    u.EmailConfirmed,
                    u.PhoneNumberConfirmed,
                    u.CreatedAt,
                    u.UpdatedAt,
                    uc.Id AS ClaimId,
                    uc.ClaimType,
                    uc.ClaimValue
                FROM 
                    users AS u
                LEFT JOIN
                    user_claims AS uc ON u.Id = uc.UserId
                WHERE 
                    u.id = ANY(@ids)";
            var command = new CommandDefinition(sql, new { ids }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            return rows
                .GroupBy(r => r.Id)
                .Select(g => MapUser(g.ToArray()))
                .Where(u => u != null)
                .Select(u => u!);
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
            UpdatedAt = firstRow.UpdatedAt,
            Claims = rows.Select(r => new UserCliamDto
            {
                Id = r.ClaimId,
                ClaimType = r.ClaimType,
                ClaimValue = r.ClaimValue
            }).ToList()
        };
        return user;
    }
}