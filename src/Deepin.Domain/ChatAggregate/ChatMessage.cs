namespace Deepin.Domain.ChatAggregate;

public class ChatMessage : Entity<Guid>
{
    public Guid ChatId { get; private set; }
    public ChatMessageType Type { get; private set; }
    public Guid MessageId { get; private set; }
    public Guid? SenderId { get; private set; }
    public DateTimeOffset SentAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public ChatMessage() { }
    public ChatMessage(ChatMessageType type, Guid messageId, DateTimeOffset sentAt, Guid? senderId = null)
    {
        Type = type;
        SenderId = senderId;
        MessageId = messageId;
        SentAt = sentAt;
    }
    public ChatMessage(Guid chatId, ChatMessageType type, Guid messageId, DateTimeOffset sentAt, Guid? senderId = null)
    {
        ChatId = chatId;
        Type = type;
        SenderId = senderId;
        MessageId = messageId;
        SentAt = sentAt;
    }
    public void Delete()
    {
        IsDeleted = true;
    }
}
