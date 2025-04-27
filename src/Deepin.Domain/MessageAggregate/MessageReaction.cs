namespace Deepin.Domain.MessageAggregate;

public class MessageReaction : Entity<Guid>, IAggregateRoot
{
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
    public string Emoji { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}