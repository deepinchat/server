namespace Deepin.Domain.ChatAggregate;

public class ChatReadStatus : Entity<Guid>
{
    public Guid ChatId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid LastReadMessageId { get; private set; }
    public DateTimeOffset LastReadAt { get; private set; }
    public ChatReadStatus()
    {
        LastReadAt = DateTimeOffset.UtcNow;
    }
    public ChatReadStatus(Guid chatId, Guid userId, Guid messageId) : this()
    {
        ChatId = chatId;
        UserId = userId;
        LastReadMessageId = messageId;
    }
    public void ReadMessage(Guid messageId)
    {
        LastReadMessageId = messageId;
        LastReadAt = DateTimeOffset.UtcNow;
    }
}
