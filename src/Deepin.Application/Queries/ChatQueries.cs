using Dapper;
using Deepin.Domain.ChatAggregate;
using Deepin.Application.Interfaces;
using Deepin.Application.DTOs.Chats;
using Deepin.Application.DTOs;
using System.Data;

namespace Deepin.Application.Queries;

public interface IChatQueries
{
    Task<ChatType?> GetChatTypeAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DirectChatDto?> GetDirectChatByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GroupChatDto?> GetGroupChatByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DirectChatDto>> GetDirectChatsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupChatDto>> GetGroupChatsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<ChatMemberDto?> GetChatMember(Guid chatId, Guid userId, CancellationToken cancellationToken = default);
    Task<IPagedResult<ChatMemberDto>> GetChatMembers(Guid chatId, int offset, int limit, CancellationToken cancellationToken = default);
}

public class ChatQueries(IDbConnectionFactory dbConnectionFactory) : IChatQueries
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<DirectChatDto?> GetDirectChatByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using (var connection = await _dbConnectionFactory.CreateChatDbConnectionAsync(cancellationToken))
        {
            var sql = @"SELECT 
                            c.id,
                            c.created_by,
                            c.created_at,
                            c.updated_at,
                            cm.id as member_id,
                            cm.user_id,
                            cm.display_name,
                            cm.joined_at,
                            cm.updated_at,
                            cm.role,
                            cm.is_muted,
                            cm.is_banned
                        FROM 
                            chats c 
                        JOIN 
                            chat_members cm ON c.id = cm.chat_id 
                        WHERE 
                            c.is_deleted = false 
                        AND
                           c.type = 0
                        AND 
                           c.id = @id";
            var command = new CommandDefinition(sql, new { id, }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            return rows is null ? null : MapDirectChatDto(rows);
        }
    }

    public async Task<IEnumerable<DirectChatDto>> GetDirectChatsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        using (var connection = await _dbConnectionFactory.CreateChatDbConnectionAsync(cancellationToken))
        {
            var sql = @"SELECT 
                            c.id,
                            c.created_by,
                            c.created_at,
                            c.updated_at,
                            cm.id as member_id,
                            cm.user_id,
                            cm.display_name,
                            cm.joined_at,
                            cm.updated_at,
                            cm.role,
                            cm.is_muted,
                            cm.is_banned
                        FROM 
                            chats c 
                        JOIN 
                            chat_members cm ON c.id = cm.chat_id
                        JOIN 
                            chat_members cm2 ON c.id = cm2.chat_id
                        WHERE 
                            c.is_deleted = false 
                        AND
                            c.type = 0
                        AND 
                            cm.user_id != @userId
                        AND
                            cm2.user_id = @userId
                        ORDER BY
                            c.updated_at DESC";
            var command = new CommandDefinition(sql, new { userId }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            return rows.GroupBy(row => row.id).Select(MapDirectChatDto).ToList();
        }
    }

    public async Task<GroupChatDto?> GetGroupChatByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using (var connection = await _dbConnectionFactory.CreateChatDbConnectionAsync(cancellationToken))
        {
            var sql = @"SELECT 
                            c.id,
                            c.created_by,
                            c.created_at,
                            c.updated_at,
                            c.name,
                            c.avatar_file_id,
                            c.description,
                            c.is_public,
                            c.user_name
                        FROM 
                            chats c 
                        WHERE 
                            c.is_deleted = false 
                        AND
                           c.type = 1
                        AND 
                           c.id = @id";
            var command = new CommandDefinition(sql, new { id }, cancellationToken: cancellationToken);
            var result = await connection.QueryFirstOrDefaultAsync<dynamic>(command);
            return result is null ? null : MapGroupChatDto(result);
        }
    }

    public async Task<IEnumerable<GroupChatDto>> GetGroupChatsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        using (var connection = await _dbConnectionFactory.CreateChatDbConnectionAsync(cancellationToken))
        {
            var sql = @"SELECT 
                            c.id,
                            c.created_at,
                            c.updated_at,
                            c.created_by,
                            c.name,
                            c.avatar_file_id,
                            c.description,
                            c.is_public,
                            c.user_name
                        FROM 
                            chats c 
                        JOIN 
                            chat_members cm ON c.id = cm.chat_id
                        WHERE 
                            c.is_deleted = false 
                        AND
                           c.type = 1
                        AND 
                           cm.user_id = @userId
                        ORDER BY
                            c.updated_at DESC";
            var command = new CommandDefinition(sql, new { userId }, cancellationToken: cancellationToken);
            var result = await connection.QueryAsync<dynamic>(command);
            return result.Select(MapGroupChatDto).ToList();
        }
    }
    public async Task<ChatMemberDto?> GetChatMember(Guid chatId, Guid userId, CancellationToken cancellationToken = default)
    {
        using (var connection = await _dbConnectionFactory.CreateChatDbConnectionAsync(cancellationToken))
        {
            var sql = @"SELECT * FROM chat_members WHERE chat_id = @chatId AND user_id = @userId";
            var command = new CommandDefinition(sql, new { chatId, userId }, cancellationToken: cancellationToken);
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(command);
            return row is not null ? MapChatMemberDto(row) : null;
        }
    }

    public async Task<IPagedResult<ChatMemberDto>> GetChatMembers(Guid chatId, int offset, int limit, CancellationToken cancellationToken = default)
    {
        using (var connection = await _dbConnectionFactory.CreateChatDbConnectionAsync(cancellationToken))
        {
            var query = @"SELECT * FROM chat_members WHERE chat_id = @chatId ORDER BY joined_at DESC OFFSET @offset LIMIT @limit";
            var countQuery = "SELECT COUNT(*) FROM chat_members WHERE chat_id = @chatId";

            var countCommand = new CommandDefinition(countQuery, new { chatId }, cancellationToken: cancellationToken);
            var queryCommand = new CommandDefinition(query, new { chatId, offset, limit }, cancellationToken: cancellationToken);

            var count = await connection.ExecuteScalarAsync<int>(countCommand);
            var rows = await connection.QueryAsync<dynamic>(queryCommand);
            return new PagedResult<ChatMemberDto>(rows.Select(MapChatMemberDto).ToList(), offset, limit, count);
        }
    }

    public async Task<ChatType?> GetChatTypeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using (var connection = await _dbConnectionFactory.CreateChatDbConnectionAsync(cancellationToken))
        {
            var sql = @"SELECT type FROM chats WHERE id = @id AND is_deleted = false";
            var command = new CommandDefinition(sql, new { id }, cancellationToken: cancellationToken);
            var result = await connection.ExecuteScalarAsync<int>(command);
            if (result == 0)
            {
                return null;
            }
            return (ChatType)result;
        }
    }

    #region Map Methods
    private static GroupChatDto MapGroupChatDto(dynamic result)
    {
        var dto = new GroupChatDto
        {
            Id = result.id,
            CreatedAt = result.created_at,
            UpdatedAt = result.updated_at,
            CreatedBy = result.created_by,
            Name = result.name,
            AvatarFileId = result.avatar_file_id,
            Description = result.description,
            IsPublic = result.is_public,
            UserName = result.user_name
        };
        return dto;
    }

    private static DirectChatDto MapDirectChatDto(IEnumerable<dynamic> rows)
    {
        var firstRow = rows.First();
        var result = new DirectChatDto
        {
            Id = firstRow.id,
            CreatedBy = firstRow.created_by,
            CreatedAt = firstRow.created_at,
            UpdatedAt = firstRow.updated_at
        };
        result.Members = rows.Select(row => new ChatMemberDto
        {
            Id = row.member_id,
            UserId = row.user_id,
            DisplayName = row.display_name,
            JoinedAt = row.joined_at,
            UpdatedAt = row.updated_at,
            Role = Enum.Parse<ChatMemberRole>(row.role, true),
            IsMuted = row.is_muted,
            IsBanned = row.is_banned
        }).ToList();
        return result;
    }

    private static ChatMemberDto MapChatMemberDto(dynamic row)
    {
        return new ChatMemberDto
        {
            UserId = row.user_id,
            DisplayName = row.display_name,
            JoinedAt = row.joined_at,
            UpdatedAt = row.updated_at,
            Id = row.id,
            IsMuted = row.is_muted,
            IsBanned = row.is_banned,
            Role = Enum.Parse<ChatMemberRole>(row.role, true)
        };
    }
    #endregion
}