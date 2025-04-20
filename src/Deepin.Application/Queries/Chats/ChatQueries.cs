using Deepin.Chatting.Application.Constants;
using Deepin.Application.Models.Chats;
using Deepin.Infrastructure.Caching;
using Deepin.Infrastructure.Pagination;
using Npgsql;
using Deepin.Infrastructure.Data;
using Dapper;
using Deepin.Domain.ChatAggregate;

namespace Deepin.Application.Queries.Chats;

public class ChatQueries(string connectionString, ICacheManager cacheManager) : QueryBase(ConversationDbContext.DEFAULT_SCHEMA), IChatQueries
{
    private readonly string _connectionString = connectionString;
    private readonly ICacheManager _cacheManager = cacheManager;

    public async Task<IEnumerable<ChatDto>> GetChats(string userId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var sql = @"SELECT c.* FROM chats c 
                        JOIN chat_members cm ON c.id = cm.chat_id
                        WHERE c.is_deleted = false AND cm.user_id = @userId";
            var result = await connection.QueryAsync<dynamic>(BuildSqlWithSchema(sql), new { userId });
            return result.Select(MapChatDto);
        }
    }
    public async Task<ChatDto?> GetChatById(Guid id)
    {
        return await _cacheManager.GetOrSetAsync<ChatDto>(CacheKeys.GetChatByIdCacheKey(id), async () =>
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var sql = @"SELECT * FROM chats WHERE id = @id";
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(BuildSqlWithSchema(sql), new { id });
                return MapChatDto(result);
            }
        });
    }
    public async Task<ChatMemberDto?> GetChatMember(Guid chatId, string userId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var sql = @"SELECT * FROM chat_members WHERE chat_id = @chatId AND user_id = @userId";
            var row = await connection.QueryFirstOrDefaultAsync<ChatMemberDto>(BuildSqlWithSchema(sql), new { chatId, userId });
            return row is not null ? MapChatMemberDto(row) : null;
        }
    }
    public async Task<IPagination<ChatMemberDto>> GetChatMembers(Guid chatId, int offset, int limit)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var query = @"SELECT * FROM chat_members WHERE chat_id = @chatId ORDER BY joined_at DESC OFFSET @offset LIMIT @limit";
            var countQuery = "SELECT COUNT(*) FROM chat_members WHERE chat_id = @chatId";
            var count = await connection.ExecuteScalarAsync<int>(BuildSqlWithSchema(countQuery), new { chatId });
            var rows = await connection.QueryAsync<dynamic>(BuildSqlWithSchema(query), new { chatId, offset, limit });
            return new Pagination<ChatMemberDto>(rows.Select(MapChatMemberDto), offset, limit, count);
        }
    }
    public async Task<IEnumerable<ChatReadStatusDto>> GetChatReadStatusesAsync(string userId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var sql = @"SELECT * FROM chat_read_statuses WHERE user_id = @userId";
            var rows = await connection.QueryAsync<dynamic>(BuildSqlWithSchema(sql), new { userId });
            return rows is null ? [] : rows.Select(MapChatReadStatusDto);
        }
    }

    public async Task<ChatReadStatusDto?> GetChatReadStatusAsync(Guid chatId, string userId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var sql = @"SELECT * FROM chat_read_statuses WHERE chat_id = @chatId AND user_id = @userId";
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(BuildSqlWithSchema(sql), new { chatId, userId });
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
            CreatedBy = result.created_by,
            IsDeleted = result.is_deleted
        };
        if (dto.Type != ChatType.Direct)
        {
            dto.GroupInfo = new ChatInfo
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
