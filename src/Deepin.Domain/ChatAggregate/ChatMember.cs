namespace Deepin.Domain.ChatAggregate;

public class ChatMember : Entity<Guid>
{
    public Guid ChatId { get; private set; }
    public Guid UserId { get; private set; }
    public string? DisplayName { get; private set; }
    public bool IsMuted { get; private set; }
    public bool IsBanned { get; private set; }
    public ChatMemberRole Role { get; private set; }
    public DateTimeOffset JoinedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public ChatMember()
    {
    }
    public ChatMember(Guid userId, ChatMemberRole role, string? displayName = null) : this()
    {
        UserId = userId;
        Role = role;
        DisplayName = displayName ?? string.Empty;
        JoinedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void UpdateRole(ChatMemberRole role)
    {
        Role = role;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void UpdateDisplayName(string displayName)
    {
        DisplayName = displayName;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Mute()
    {
        IsMuted = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Unmute()
    {
        IsMuted = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Ban()
    {
        IsBanned = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Unban()
    {
        IsBanned = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
