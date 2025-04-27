namespace Deepin.Domain.ChatAggregate;

public class ChatReadStatus : Entity<Guid>
{
    public Guid ChatId { get; private set; }
    public Guid UserId { get; private set; }
    public string LastReadMessageId { get; private set; }
    public DateTimeOffset LastReadAt { get; private set; }
    public ChatReadStatus()
    {
        LastReadMessageId = string.Empty;
        LastReadAt = DateTimeOffset.UtcNow;
    }
    public ChatReadStatus(Guid chatId, Guid userId, string messageId) : this()
    {
        ChatId = chatId;
        UserId = userId;
        LastReadMessageId = messageId;
    }
    public void ReadMessage(string messageId)
    {
        LastReadMessageId = messageId;
        LastReadAt = DateTimeOffset.UtcNow;
    }
}
