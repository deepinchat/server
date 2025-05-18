using Deepin.Chatting.Application.Constants;
using Dapper;
using Deepin.Domain.ChatAggregate;
using Deepin.Application.Interfaces;
using Deepin.Application.DTOs.Chats;
using Deepin.Application.DTOs;
using System.Data;

namespace Deepin.Application.Queries;
public interface IChatQueries
{
    Task<IEnumerable<ChatDto>> GetChats(Guid userId, CancellationToken cancellationToken = default);
    Task<ChatDto?> GetChatById(Guid id, CancellationToken cancellationToken = default);
    Task<ChatMemberDto?> GetChatMember(Guid chatId, Guid userId, CancellationToken cancellationToken = default);
    Task<IPagedResult<ChatMemberDto>> GetChatMembers(Guid chatId, int offset, int limit, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChatReadStatusDto>> GetChatReadStatusesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ChatReadStatusDto?> GetChatReadStatusAsync(Guid chatId, Guid userId, CancellationToken cancellationToken = default);
}

public class ChatQueries(IDbConnectionFactory dbConnectionFactory, ICacheManager cacheManager) : IChatQueries
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
    private readonly ICacheManager _cacheManager = cacheManager;

    public async Task<IEnumerable<ChatDto>> GetChats(Guid userId, CancellationToken cancellationToken = default)
    {
        using (var connection = await _dbConnectionFactory.CreateChatDbConnectionAsync(cancellationToken))
        {
            var sql = @"SELECT 
                            c.*
                        FROM 
                            chats c 
                        JOIN 
                            chat_members cm ON c.id = cm.chat_id
                        WHERE 
                            c.is_deleted = false AND cm.user_id = @userId";
            var command = new CommandDefinition(sql, new { userId }, cancellationToken: cancellationToken);
            var result = await connection.QueryAsync<dynamic>(command);
            return result.Select(MapChatDto);
        }
    }

    public async Task<ChatDto?> GetChatById(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeys.GetChatByIdCacheKey(id);
        return await _cacheManager.GetOrSetAsync<ChatDto?>(cacheKey, async () =>
        {
            using (var connection = await _dbConnectionFactory.CreateChatDbConnectionAsync(cancellationToken))
            {
                var sql = @"
                SELECT 
                    * 
                FROM 
                    chats 
                WHERE 
                    id = @id 
                AND 
                    is_deleted = false";
                var command = new CommandDefinition(sql, new { id }, cancellationToken: cancellationToken);
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(command);
                return result is null ? null : MapChatDto(result);
            }
        });
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

    public async Task<IEnumerable<ChatReadStatusDto>> GetChatReadStatusesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        using (var connection = await _dbConnectionFactory.CreateChatDbConnectionAsync(cancellationToken))
        {
            var sql = @"SELECT * FROM chat_read_statuses WHERE user_id = @userId";
            var command = new CommandDefinition(sql, new { userId }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            return rows is null ? [] : rows.Select(MapChatReadStatusDto);
        }
    }

    public async Task<ChatReadStatusDto?> GetChatReadStatusAsync(Guid chatId, Guid userId, CancellationToken cancellationToken = default)
    {
        using (var connection = await _dbConnectionFactory.CreateChatDbConnectionAsync(cancellationToken))
        {
            var sql = @"SELECT * FROM chat_read_statuses WHERE chat_id = @chatId AND user_id = @userId";
            var command = new CommandDefinition(sql, new { chatId, userId }, cancellationToken: cancellationToken);
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(command);
            return row is null ? null : MapChatReadStatusDto(row);
        }
    }

    private ChatReadStatusDto MapChatReadStatusDto(dynamic row)
    {
        return new ChatReadStatusDto
        {
            ChatId = row.chat_id,
            UserId = row.user_id,
            LastReadMessageId = row.last_read_message_id,
            LastReadAt = row.last_read_at
        };
    }

    private ChatMemberDto MapChatMemberDto(dynamic row)
    {
        return new ChatMemberDto
        {
            UserId = row.user_id,
            DisplayName = row.display_name,
            JoinedAt = row.joined_at,
            UpdatedAt = row.updated_at,
            Role = Enum.Parse<ChatMemberRole>(row.role, true)
        };
    }

    private ChatDto MapChatDto(dynamic result)
    {
        var dto = new ChatDto
        {
            Id = result.id,
            Type = Enum.Parse<ChatType>(result.type, true),
            CreatedAt = result.created_at,
            UpdatedAt = result.updated_at,
            CreatedBy = result.created_by
        };
        if (dto.Type != ChatType.Direct)
        {
            dto.GroupInfo = new ChatGroupInfoDto
            {
                Name = result.name,
                AvatarFileId = result.avatar_file_id,
                Description = result.description,
                IsPublic = result.is_public,
                UserName = result.user_name
            };
        }
        return dto;
    }
}