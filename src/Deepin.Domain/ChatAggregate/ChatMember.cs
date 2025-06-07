namespace Deepin.Domain.ChatAggregate;
public class ChatMember : Entity<Guid>
{
    public Guid UserId { get; private set; }
    public string? DisplayName { get; private set; }
    public DateTimeOffset JoinedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public ChatMemberRole Role { get; private set; }
    public ChatMember()
    {
        JoinedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public ChatMember(Guid userId, ChatMemberRole role, string? displayName = null) : this()
    {
        UserId = userId;
        Role = role;
        DisplayName = displayName ?? string.Empty;
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
}
