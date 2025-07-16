namespace Deepin.Domain.ChatAggregate;

public class ChatJoinRequest : Entity<Guid>
{
    public Guid ChatId { get; private set; }
    public Guid UserId { get; private set; }
    public string? Message { get; private set; }
    public ChatJoinRequestStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public Guid? ReviewedBy { get; private set; }
    public DateTimeOffset? ReviewedAt { get; private set; }
    private ChatJoinRequest()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        ExpiresAt = CreatedAt.AddDays(7);
        Status = ChatJoinRequestStatus.Pending;
    }
    public ChatJoinRequest(Guid chatId, Guid userId, string? message = null) : this()
    {
        ChatId = chatId;
        UserId = userId;
        Message = message;
        Status = ChatJoinRequestStatus.Pending;
    }
}
