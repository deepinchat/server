namespace Deepin.Domain.ChatAggregate;

public class ChatReadStatus : Entity<Guid>
{
    public Guid ChatId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? LastReadMessageId { get; private set; }
    public DateTimeOffset LastReadAt { get; private set; }
    public ChatReadStatus() { }
    public ChatReadStatus(Guid userId, Guid? messageId = null) : this()
    {
        UserId = userId;
        if (messageId.HasValue)
        {
            ReadMessage(messageId.Value);
        }
    }
    public ChatReadStatus(Guid chatId, Guid userId, Guid? messageId = null) : this(userId, messageId)
    {
        ChatId = chatId;
    }
    public void ReadMessage(Guid messageId)
    {
        LastReadMessageId = messageId;
        LastReadAt = DateTimeOffset.UtcNow;
    }
}
