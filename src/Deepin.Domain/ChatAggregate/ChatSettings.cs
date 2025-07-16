namespace Deepin.Domain.ChatAggregate;

public class ChatSettings : Entity<Guid>
{
    public Guid ChatId { get; private set; }
    public Guid UserId { get; private set; }
    public bool IsPinned { get; private set; }
    public bool IsMuted { get; private set; }
    public ChatNotificationLevel NotificationLevel { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public ChatSettings() { }
    public ChatSettings(Guid userId, bool isPinned = false, bool isMuted = false, ChatNotificationLevel notificationLevel = ChatNotificationLevel.All) : this()
    {
        UserId = userId;
        IsPinned = isPinned;
        IsMuted = isMuted;
        NotificationLevel = notificationLevel;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Pin()
    {
        IsPinned = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Unpin()
    {
        IsPinned = false;
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
    public void UpdateNotificationLevel(ChatNotificationLevel level)
    {
        NotificationLevel = level;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
