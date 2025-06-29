using Deepin.Internal.SDK.Enums;

namespace Deepin.Internal.SDK.Models;

public class GroupChatDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Description { get; set; }
    public Guid? AvatarFileId { get; set; }
    public bool IsPublic { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DirectChatDto
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<ChatMemberDto> Members { get; set; } = [];
}

public class ChatMemberDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? DisplayName { get; set; }
    public bool IsMuted { get; set; }
    public bool IsBanned { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ChatMemberRole Role { get; set; }
}

public record SearchChatRequest(
    string? Search = null,
    int Offset = 0,
    int Limit = 20);

public record CreateGroupChatRequest(
    string Name,
    string? UserName,
    string? Description,
    Guid? AvatarFileId,
    bool IsPublic);

public record ChatSettingsRequest(
    Guid ChatId,
    ChatNotificationLevel NotificationLevel,
    bool IsMuted,
    bool IsPinned);

public record UpdateGroupChatRequest(
    Guid Id,
    string Name,
    string? UserName,
    string? Description,
    Guid? AvatarFileId,
    bool IsPublic);


public class ChatUnreadCountDto
{
    public Guid ChatId { get; set; }
    public long UnreadCount { get; set; }
}
