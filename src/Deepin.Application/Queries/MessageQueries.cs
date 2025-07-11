using Dapper;
using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Messages;
using Deepin.Application.Interfaces;
using Deepin.Domain.MessageAggregate;
using Newtonsoft.Json;

namespace Deepin.Application.Queries;

public interface IMessageQueries
{
    Task<MessageDto?> GetMessageAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid[] ids, CancellationToken cancellationToken = default);
    Task<Guid?> GetLastIdAsync(Guid chatId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LastMessageDto>> GetLastMessageIdsAsync(Guid[] chatIds, CancellationToken cancellationToken = default);
    Task<IPagedResult<MessageDto>> SearchMessagesAsync(
        int limit,
        int offset,
        string? search = null,
        Guid? chatId = null,
        Guid? userId = null,
        SortDirection sortBy = SortDirection.Desc,
        DateTimeOffset? readAt = null,
        CancellationToken cancellationToken = default);
}

public class MessageQueries(IDbConnectionFactory dbConnectionFactory) : IMessageQueries
{
    public async Task<IEnumerable<LastMessageDto>> GetLastMessageIdsAsync(Guid[] chatIds, CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateMessageDbConnectionAsync(cancellationToken))
        {
            var sql = @"
                SELECT DISTINCT ON (chat_id)
                    chat_id,
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
                return [];
            }
            return rows.Select(row => new LastMessageDto
            {
                ChatId = row.chat_id,
                MessageId = row.id
            });
        }
    }
    public async Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateMessageDbConnectionAsync(cancellationToken))
        {
            var sql = @"
                SELECT
                    m.id,
                    m.type,
                    m.content,
                    m.created_at,
                    m.updated_at,
                    m.user_id,
                    m.chat_id,
                    m.is_deleted,
                    m.is_read,
                    m.is_edited,
                    m.is_pinned,
                    m.metadata,
                    m.parent_id,
                    m.reply_to_id,
                    m.mentions,
                    ma.id AS attachment_id,
                    ma.type AS attachment_type,
                    ma.file_id AS attachment_file_id,
                    ma.file_name AS attachment_file_name,
                    ma.file_size AS attachment_file_size,
                    ma.content_type AS attachment_content_type,
                    ma.order AS attachment_order,
                    ma.thumbnail_file_id AS attachment_thumbnail_file_id,
                    ma.metadata AS attachment_metadata
                FROM messages m
                LEFT JOIN message_attachments ma ON m.id = ma.message_id
                WHERE 
                    m.id = ANY(@ids) 
                AND 
                    NOT m.is_deleted";
            var command = new CommandDefinition(sql, new { ids }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            return rows is null || rows.Any() == false ? [] : rows.ToList().GroupBy(row => row.id).Select(MapMessageDto);
        }
    }
    public async Task<MessageDto?> GetMessageAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var messages = await GetMessagesAsync([id], cancellationToken);
        return messages?.FirstOrDefault();
    }

    public async Task<IPagedResult<MessageDto>> SearchMessagesAsync(
        int limit,
        int offset,
        string? search = null,
        Guid? chatId = null,
        Guid? userId = null,
        SortDirection sortBy = SortDirection.Desc,
        DateTimeOffset? readAt = null,
        CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateMessageDbConnectionAsync(cancellationToken))
        {
            var countSql = "SELECT COUNT(*) FROM messages m WHERE NOT m.is_deleted";
            var querySql = $@"
                SELECT
                    m.id,
                    m.type,
                    m.content,
                    m.created_at,
                    m.updated_at,
                    m.user_id,
                    m.chat_id,
                    m.is_deleted,
                    m.is_read,
                    m.is_edited,
                    m.is_pinned,
                    m.metadata,
                    m.parent_id,
                    m.reply_to_id,
                    m.mentions,
                    ma.id AS attachment_id,
                    ma.type AS attachment_type,
                    ma.file_id AS attachment_file_id,
                    ma.file_name AS attachment_file_name,
                    ma.file_size AS attachment_file_size,
                    ma.content_type AS attachment_content_type,
                    ma.order AS attachment_order,
                    ma.thumbnail_file_id AS attachment_thumbnail_file_id,
                    ma.metadata AS attachment_metadata
                FROM messages m
                LEFT JOIN message_attachments ma ON m.id = ma.message_id
                WHERE NOT m.is_deleted";

            var condations = new List<string>();
            if (chatId.HasValue)
            {
                condations.Add("m.chat_id = @chatId");
            }
            if (userId.HasValue)
            {
                condations.Add("m.user_id = @userId");
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                condations.Add("m.content ILIKE @search");
            }
            if (readAt.HasValue)
            {
                if (sortBy == SortDirection.Desc)
                {
                    condations.Add("m.created_at <= @readAt");
                }
                else
                {
                    condations.Add("m.created_at >= @readAt");
                }
            }
            if (condations.Any())
            {
                countSql += " AND " + string.Join(" AND ", condations);
                querySql += " AND " + string.Join(" AND ", condations);
            }
            var countCommand = new CommandDefinition(countSql, new { chatId, userId, search }, cancellationToken: cancellationToken);
            var count = await connection.ExecuteScalarAsync<int>(countCommand);
            if (count == 0)
            {
                return new PagedResult<MessageDto>();
            }
            querySql += " ORDER BY m.created_at " + sortBy.ToString().ToUpperInvariant() ;
            querySql += " LIMIT @limit OFFSET @offset";
            var command = new CommandDefinition(querySql, new { limit, offset, chatId, userId, search = $"%{search}%" }, cancellationToken: cancellationToken);
            var rows = await connection.QueryAsync<dynamic>(command);
            var messages = rows.GroupBy(row => row.id).Select(MapMessageDto);
            return new PagedResult<MessageDto>(messages, offset, limit, count);
        }
    }

    private MessageDto MapMessageDto(IEnumerable<dynamic> rows)
    {
        var message = rows.First();
        var messageDto = new MessageDto
        {
            Id = message.id,
            Type = Enum.Parse<MessageType>(message.type, true),
            CreatedAt = message.created_at,
            UpdatedAt = message.updated_at,
            UserId = message.user_id,
            ChatId = message.chat_id,
            Content = message.content,
            IsDeleted = message.is_deleted,
            IsRead = message.is_read,
            IsEdited = message.is_edited,
            IsPinned = message.is_pinned,
            Metadata = message.metadata,
            ParentId = message.parent_id,
            ReplyToId = message.reply_to_id,
            Mentions = message.mentions is null ? [] : JsonConvert.DeserializeObject<IEnumerable<MessageMentionDto>>(message.mentions),
            Attachments = []
        };
        foreach (var row in rows)
        {
            if (row.attachment_id is null)
            {
                continue;
            }
            var attachment = new MessageAttachmentDto
            {
                Id = row.attachment_id,
                Type = Enum.Parse<AttachmentType>(row.attachment_type, true),
                FileId = row.attachment_file_id,
                FileName = row.attachment_file_name,
                FileSize = row.attachment_file_size,
                ContentType = row.attachment_content_type,
                Order = row.attachment_order,
                ThumbnailFileId = row.attachment_thumbnail_file_id,
                Metadata = row.attachment_metadata
            };
            messageDto.Attachments.Add(attachment);
        }

        return messageDto;
    }

    public async Task<Guid?> GetLastIdAsync(Guid chatId, CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateMessageDbConnectionAsync(cancellationToken))
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