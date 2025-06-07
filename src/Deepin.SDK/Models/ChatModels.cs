namespace Deepin.SDK.Models;

/// <summary>
/// Represents a chat in the system
/// </summary>
public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ChatType Type { get; set; }
    public int CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ChatMember>? Members { get; set; }
    public Message? LastMessage { get; set; }
    public int UnreadCount { get; set; }
}

/// <summary>
/// Represents the type of chat
/// </summary>
public enum ChatType
{
    Group = 0,
    Direct = 1
}

/// <summary>
/// Represents a member of a chat
/// </summary>
public class ChatMember
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public ChatMemberRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
    public User? User { get; set; }
}

/// <summary>
/// Represents the role of a chat member
/// </summary>
public enum ChatMemberRole
{
    Member = 0,
    Admin = 1,
    Owner = 2
}

/// <summary>
/// Request model for creating a chat
/// </summary>
public class CreateChatRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ChatType Type { get; set; }
    public List<int>? MemberIds { get; set; }
}

/// <summary>
/// Request model for updating a chat
/// </summary>
public class UpdateChatRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Request model for creating a direct chat
/// </summary>
public class CreateDirectChatRequest
{
    public int UserId { get; set; }
}

/// <summary>
/// Request model for joining a chat
/// </summary>
public class JoinChatRequest
{
    public int ChatId { get; set; }
}

/// <summary>
/// Represents read status for a chat
/// </summary>
public class ReadStatus
{
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public int? LastReadMessageId { get; set; }
    public DateTime? LastReadAt { get; set; }
}

/// <summary>
/// Request model for updating read status
/// </summary>
public class UpdateReadStatusRequest
{
    public int MessageId { get; set; }
}
