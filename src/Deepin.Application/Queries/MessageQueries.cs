using Dapper;
using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Messages;
using Deepin.Application.Interfaces;
using Deepin.Application.Requests.Messages;
using Newtonsoft.Json;

namespace Deepin.Application.Queries;

public interface IMessageQueries
{
    Task<MessageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid?> GetLastIdAsync(Guid chatId, CancellationToken cancellationToken = default);
    Task<IPagedResult<MessageDto>> GetPagedAsync(SearchMessageRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<MessageDto>> BatchGetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<IDictionary<Guid, Guid>> GetLastIdsAsync(IEnumerable<Guid> chatIds, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(Guid chatId, DateTimeOffset? lastReadTime = null, CancellationToken cancellationToken = default);
}

public class MessageQueries(IDbConnectionFactory dbConnectionFactory) : IMessageQueries
{
    public async Task<int> GetUnreadCountAsync(Guid chatId, DateTimeOffset? lastReadTime = null, CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateStorageDbConnectionAsync(cancellationToken))
        {
            var sql = @"
                SELECT 
                    COUNT(*)
                FROM 
                    messages
                WHERE 
                    chat_id = @chatId AND NOT is_deleted";
            if (lastReadTime.HasValue)
            {
                sql += " AND created_at > @lastReadTime";
            }
            var command = new CommandDefinition(sql, new { chatId, lastReadTime }, cancellationToken: cancellationToken);
            return await connection.ExecuteScalarAsync<int>(command);
        }
    }
    public async Task<IDictionary<Guid, Guid>> GetLastIdsAsync(IEnumerable<Guid> chatIds, CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateStorageDbConnectionAsync(cancellationToken))
        {
            var sql = @"
                SELECT DISTINCT ON (chat_id)
                    chat_id
                    id
                FROM 
                    messages
                WHERE 
                    chat_id = ANY(@chatIds) AND NOT is_deleted
                ORDER BY 
                    chat_id, created_at DESC";
            var command = new CommandDefinition(sql, new { chatIds }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            if (rows is null || rows.Any() == false)
            {
                return new Dictionary<Guid, Guid>();
            }
            return rows.ToDictionary(row => (Guid)row.chat_id, row => (Guid)row.id);
        }
    }
    public async Task<IEnumerable<MessageDto>> BatchGetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateStorageDbConnectionAsync(cancellationToken))
        {
            var sql = @"
                SELECT
                    m.id,
                    m.type,
                    m.content,
                    m.created_at,
                    m.updated_at,
                    m.user_id,
                    m.receiver_id,
                    m.is_deleted,
                    m.is_read,
                    m.is_edited,
                    m.is_pinned,
                    m.metadata,
                    m.parent_id,
                    m.reply_to_id
                    m.mentions,
                    a.attachment_id,
                    a.attachment_type,
                    a.attachment_file_id,
                    a.attachment_file_name,
                    a.attachment_file_size,
                    a.attachment_content_type,
                    a.attachment_order,
                    a.attachment_thumbnail_file_id,
                    a.attachment_metadata
                FROM messages m
                LEFT JOIN attachments a ON m.id = m2.message_id
                WHERE m.is_deleted IS false AND m.id = ANY(@ids)";
            var command = new CommandDefinition(sql, new { ids }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            return rows is null || rows.Any() == false ? [] : rows.GroupBy(row => row.id).Select(MapMessageDto);
        }
    }
    public async Task<MessageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var messages = await BatchGetByIdsAsync([id], cancellationToken);
        return messages.FirstOrDefault();
    }

    public async Task<IPagedResult<MessageDto>> GetPagedAsync(SearchMessageRequest request, CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateStorageDbConnectionAsync(cancellationToken))
        {
            var condationSql = " WHERE m.is_deleted IS false";
            if (request.ChatId.HasValue)
            {
                condationSql += " AND m.chat_id = @chatId";
            }
            if (request.SenderId.HasValue)
            {
                condationSql += " AND m.user_id = @senderId";
            }
            if (!string.IsNullOrEmpty(request.Search))
            {
                condationSql += " AND m.content ILIKE '%' || @search || '%'";
            }
            var countSql = $@"
                SELECT COUNT(*) FROM messages m
                {condationSql}";
            var countCommand = new CommandDefinition(countSql, new { chatId = request.ChatId, senderId = request.SenderId, search = request.Search }, cancellationToken: cancellationToken);
            var count = await connection.ExecuteScalarAsync<int>(countCommand);
            if (count == 0)
            {
                return new PagedResult<MessageDto>();
            }
            var querySql = $@"
                SELECT
                    m.id,
                    m.type,
                    m.content,
                    m.created_at,
                    m.updated_at,
                    m.user_id,
                    m.receiver_id,
                    m.is_deleted,
                    m.is_read,
                    m.is_edited,
                    m.is_pinned,
                    m.metadata,
                    m.parent_id,
                    m.reply_to_id
                    m.mentions,
                    a.attachment_id,
                    a.attachment_type,
                    a.attachment_file_id,
                    a.attachment_file_name,
                    a.attachment_file_size,
                    a.attachment_content_type,
                    a.attachment_order,
                    a.attachment_thumbnail_file_id,
                    a.attachment_metadata
                FROM messages m
                LEFT JOIN attachments a ON m.id = m2.message_id
                {condationSql}
                ORDER BY m.created_at DESC
                OFFSET @offset LIMIT @limit";
            var command = new CommandDefinition(querySql, new
            {
                chatId = request.ChatId,
                senderId = request.SenderId,
                search = request.Search,
                offset = request.Offset,
                limit = request.Limit
            }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            var messages = rows.GroupBy(row => row.id).Select(MapMessageDto);
            return new PagedResult<MessageDto>(messages, request.Offset, request.Limit, count);
        }
    }

    private MessageDto MapMessageDto(IEnumerable<dynamic> rows)
    {
        var message = rows.First();
        var messageDto = new MessageDto
        {
            Id = message.id,
            Type = message.type,
            CreatedAt = message.created_at,
            ModifiedAt = message.updated_at,
            UserId = message.sender_id,
            ChatId = message.receiver_id,
            Text = message.content,
            IsDeleted = message.is_deleted,
            IsRead = message.is_read,
            IsEdited = message.is_edited,
            IsPinned = message.is_pinned,
            Metadata = message.metadata,
            ParentId = message.parent_id,
            ReplyToId = message.reply_to_id,
            Mentions = message.mentions is null ? [] : JsonConvert.DeserializeObject<IEnumerable<MessageMentionDto>>(message.mentions),
            Attachments = rows.Select(row => new MessageAttachmentDto
            {
                Id = row.id,
                Type = row.type,
                FileId = row.file_id,
                FileName = row.file_name,
                FileSize = row.file_size,
                ContentType = row.content_type,
                Order = row.order,
                ThumbnailFileId = row.thumbnail_file_id,
                Metadata = row.metadata
            })
        }; ;
        return messageDto;
    }

    public async Task<Guid?> GetLastIdAsync(Guid chatId, CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateStorageDbConnectionAsync(cancellationToken))
        {
            var sql = @"
                SELECT 
                    id
                FROM 
                    messages
                WHERE 
                    chat_id = @chatId AND NOT is_deleted
                ORDER BY 
                    created_at DESC
                LIMIT 1";
            var command = new CommandDefinition(sql, new { chatId }, cancellationToken: cancellationToken);
            return await connection.ExecuteScalarAsync<Guid?>(command);
        }
    }
}
