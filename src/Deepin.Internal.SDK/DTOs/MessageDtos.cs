using Deepin.Internal.SDK.Enums;

namespace Deepin.Internal.SDK.Models;

/// <summary>
/// Represents a message in the system
/// </summary>
public class MessageDto
{
    public Guid Id { get; set; }
    public MessageType Type { get; set; }
    public Guid ChatId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? ReplyToId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string? Text { get; set; }
    public string? Metadata { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsRead { get; set; }
    public bool IsEdited { get; set; }
    public bool IsPinned { get; set; }
    public List<MessageAttachmentDto> Attachments { get; set; } = [];
    public IEnumerable<MessageMentionDto> Mentions { get; set; } = [];
}

public class MessageAttachmentDto : MessageAttachmentRequest
{
    public Guid Id { get; set; }
}
public class MessageMentionDto : MessageMentionRequest
{
}

/// <summary>
/// Request model for sending a message
/// </summary>
public class MessageRequest
{
    public string Content { get; set; } = string.Empty;
    public Guid ChatId { get; set; }
    public MessageType Type { get; set; } = MessageType.Text;
    public List<int>? FileIds { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? ReplyToId { get; set; }
    public IEnumerable<MessageAttachmentRequest>? Attachments { get; set; }
    public IEnumerable<MessageMentionRequest>? Mentions { get; set; }
    public object? Metadata { get; set; }
}
public class MessageMentionRequest
{
    public MentionType Type { get; set; }
    public Guid? UserId { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
}

public class MessageAttachmentRequest
{
    public AttachmentType Type { get; set; }
    public int Order { get; set; }
    public Guid FileId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public Guid? ThumbnailFileId { get; set; }
    public object? Metadata { get; set; }
}
/// <summary>
/// Request model for searching messages
/// </summary>
public class SearchMessagesRequest
{
    public int Limit { get; set; } = 20;
    public int Offset { get; set; } = 0;
    public string? Query { get; set; }
    public Guid? ChatId { get; set; }
    public Guid? UserId { get; set; }
}

/// <summary>
/// Request model for getting last messages
/// </summary>
public class GetLastMessagesRequest
{
    public List<Guid> ChatIds { get; set; } = new();
}
public class BatchGetMessageRequest
{
    public List<Guid> Ids { get; set; } = new();
}
public class LastMessageDto
{
    public Guid ChatId { get; set; }
    public Guid MessageId { get; set; }
}