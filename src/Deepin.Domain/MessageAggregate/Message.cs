namespace Deepin.Domain.MessageAggregate;

public class Message : Entity<Guid>, IAggregateRoot
{
    private List<MessageAttachment> _attachments = [];
    public MessageType Type { get; set; }
    public Guid ChatId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? ReplyToId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string? Content { get; private set; }
    public string? Mentions { get; private set; }
    public string? Metadata { get; private set; }
    public bool IsDeleted { get; set; }
    public bool IsRead { get; set; }
    public bool IsEdited { get; set; }
    public bool IsPinned { get; set; }
    public IReadOnlyCollection<MessageAttachment> Attachments => _attachments.ToList();
    public Message()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public Message(Guid chatId, MessageType type, Guid? userId = null, string? content = null, Guid? parentId = null, Guid? replyToId = null, string? mentions = null, string? metadata = null) : this()
    {
        ChatId = chatId;
        Type = type;
        UserId = userId;
        Content = content;
        Metadata = metadata;
        ParentId = parentId;
        ReplyToId = replyToId;
        Mentions = mentions;
    }
    public void AddAttachment(MessageAttachment attachment)
    {
        _attachments.Add(attachment);
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Edit(string content, string? metadata = null)
    {
        Content = content;
        Metadata = metadata;
        IsEdited = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void MarkAsRead()
    {
        IsRead = true;
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
}
