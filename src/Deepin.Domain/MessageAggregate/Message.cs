namespace Deepin.Domain.MessageAggregate;

public class Message : Entity<Guid>, IAggregateRoot
{
    private List<MessageAttachment> _attachments = [];
    private List<MessageMention> _mentions = [];
    public MessageType Type { get; set; }
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? ReplyToId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public string? Text { get; private set; }
    public string? Metadata { get; private set; }
    public bool IsDeleted { get; set; }
    public bool IsRead { get; set; }
    public bool IsEdited { get; set; }
    public bool IsPinned { get; set; }
    public IReadOnlyCollection<MessageAttachment> Attachments => _attachments.ToList();
    public IReadOnlyCollection<MessageMention> Mentions => _mentions.ToList();
    public Message()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        ModifiedAt = DateTimeOffset.UtcNow;
    }
    public Message(MessageType type, Guid chatId, Guid userId, string? text = null, Guid? parentId = null, Guid? replyToId = null, string? metadata = null) : this()
    {
        Type = type;
        ChatId = chatId;
        UserId = userId;
        Text = text;
        Metadata = metadata;
        ParentId = parentId;
        ReplyToId = replyToId;
    }
    public void AddAttachment(MessageAttachment attachment)
    {
        _attachments.Add(attachment);
        ModifiedAt = DateTimeOffset.UtcNow;
    }
    public void AddMention(MessageMention mention)
    {
        _mentions.Add(mention);
        ModifiedAt = DateTimeOffset.UtcNow;
    }
    public void Edit(string text, string? metadata = null)
    {
        Text = text;
        Metadata = metadata;
        IsEdited = true;
        ModifiedAt = DateTimeOffset.UtcNow;
    }
    public void Delete()
    {
        IsDeleted = true;
        ModifiedAt = DateTimeOffset.UtcNow;
    }
    public void MarkAsRead()
    {
        IsRead = true;
        ModifiedAt = DateTimeOffset.UtcNow;
    }
    public void Pin()
    {
        IsPinned = true;
        ModifiedAt = DateTimeOffset.UtcNow;
    }
    public void Unpin()
    {
        IsPinned = false;
        ModifiedAt = DateTimeOffset.UtcNow;
    }
}
