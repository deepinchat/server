namespace Deepin.Domain.ChatAggregate;

public class ChatReadStatus : Entity<Guid>
{
    public Guid ChatId { get; private set; }
    public string UserId { get; private set; }
    public string LastReadMessageId { get; private set; }
    public DateTime LastReadAt { get; private set; }
    public ChatReadStatus()
    {
        UserId = string.Empty;
        LastReadMessageId = string.Empty;
        LastReadAt = DateTime.UtcNow;
    }
    public ChatReadStatus(Guid chatId, string userId, string messageId) : this()
    {
        ChatId = chatId;
        UserId = userId;
        LastReadMessageId = messageId;
    }
    public void ReadMessage(string messageId)
    {
        LastReadMessageId = messageId;
        LastReadAt = DateTime.UtcNow;
    }
}
