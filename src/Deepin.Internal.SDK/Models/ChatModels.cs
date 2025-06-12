namespace Deepin.Internal.SDK.Models;

/// <summary>
/// Represents a chat in the system
/// </summary>
public class Chat
{
    public Guid Id { get; set; }
    public ChatType Type { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ChatGroupInfo? GroupInfo { get; set; }
}
public class DirectChat
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid UserId { get; set; }
}
public class ChatGroupInfo
{
    public string Name { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Description { get; set; }
    public Guid? AvatarFileId { get; set; }
    public bool IsPublic { get; set; }

}

/// <summary>
/// Represents the type of chat
/// </summary>
public enum ChatType
{
    Group,
    Direct
}

/// <summary>
/// Represents a member of a chat
/// </summary>
public class ChatMember
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? DisplayName { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ChatMemberRole Role { get; set; }
}

/// <summary>
/// Represents the role of a chat member
/// </summary>
public enum ChatMemberRole
{
    Member,
    Admin,
    Owner
}

/// <summary>
/// Request model for creating a chat
/// </summary>
public class CreateChatRequest
{
    public string Name { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Description { get; set; }
    public Guid? AvatarFileId { get; set; }
    public bool IsPublic { get; set; }
    public ChatType Type { get; set; }
}

/// <summary>
/// Request model for updating a chat
/// </summary>
public class UpdateChatRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Description { get; set; }
    public Guid? AvatarFileId { get; set; }
    public bool IsPublic { get; set; }
}

/// <summary>
/// Request model for creating a direct chat
/// </summary>
public class CreateDirectChatRequest
{
    public Guid[] UserIds { get; set; } = [];
}
// <summary>
// Represents a request to search for chats
// </summary>
public class SearchChatRequest
{
    public int Limit { get; set; } = 20;
    public int Offset { get; set; } = 0;
    public string? Search { get; set; }
    public ChatType? Type { get; set; }
}
/// <summary>
/// Represents read status for a chat
/// </summary>
public class ReadStatus
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public Guid? LastReadMessageId { get; set; }
    public DateTime? LastReadAt { get; set; }
}

/// <summary>
/// Request model for updating read status
/// </summary>
public class UpdateReadStatusRequest
{
    public Guid MessageId { get; set; }
}
