namespace Deepin.SDK.Models;

/// <summary>
/// Represents a message in the system
/// </summary>
public class Message
{
    public Guid Id { get; set; }
    public MessageType Type { get; set; }
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
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
    public List<MessageAttachment> Attachments { get; set; } = [];
    public IEnumerable<MessageMention> Mentions { get; set; } = [];
}

/// <summary>
/// Represents the type of message
/// </summary>
public enum MessageType
{
    Text = 0,
    File = 1,
    Image = 2,
    System = 3
}
public class MessageAttachment : MessageAttachmentRequest
{
    public Guid Id { get; set; }
}
public class MessageMention : MessageMentionRequest
{
}


/// <summary>
/// Request model for sending a message
/// </summary>
public class SendMessageRequest
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
public enum MentionType
{
    User,
    All
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
public enum AttachmentType
{
    Image,
    Video,
    Audio,
    Document,
    File
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
    public List<int> ChatIds { get; set; } = new();
}
public class GetUnreadMessageCountRequest
{
    public Guid ChatId { get; set; }
    public DateTimeOffset? LastReadAt { get; set; }
}

/// <summary>
/// Chat unread message count response model
/// </summary>
public class ChatMessageUnreadCount
{
    public Guid ChatId { get; set; }
    public int UnreadCount { get; set; }
    public DateTimeOffset LastReadAt { get; set; }
}