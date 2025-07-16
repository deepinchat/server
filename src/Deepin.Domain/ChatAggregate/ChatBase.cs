using Deepin.Domain.Events;

namespace Deepin.Domain.ChatAggregate;

public abstract class ChatBase : Entity<Guid>, IAggregateRoot
{
    private List<ChatMember> _members = [];
    private List<ChatSettings> _settings = [];
    private List<ChatReadStatus> _readStatuses = [];
    private List<ChatMessage> _messages = [];
    public Guid CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public IReadOnlyCollection<ChatMember> Members => _members;
    public IReadOnlyCollection<ChatSettings> Settings => _settings;
    public IReadOnlyCollection<ChatReadStatus> ReadStatuses => _readStatuses;
    public IReadOnlyCollection<ChatMessage> Messages => _messages;
    protected ChatBase() { }
    public ChatBase(Guid createdBy, ChatType type)
    {
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        IsActive = true;
        CreatedBy = createdBy;
        this.AddMember(createdBy, ChatMemberRole.Owner);
        this.AddDomainEvent(new ChatCreatedDomainEvent(this, type));
    }
    public void AddMember(Guid userId, ChatMemberRole role = ChatMemberRole.Member, string? displayName = null)
    {
        if (_members.Any(m => m.UserId == userId))
        {
            throw new InvalidOperationException("User is already a member of the chat.");
        }

        _members.Add(new ChatMember(userId, role, displayName));
        _settings.Add(new ChatSettings(userId));
        _readStatuses.Add(new ChatReadStatus(userId));
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void RemoveMember(Guid userId)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId);
        if (member is null)
        {
            throw new InvalidOperationException("User is not a member of the chat.");
        }

        _members.Remove(member);
        var settings = _settings.FirstOrDefault(s => s.UserId == userId);
        if (settings is not null)
        {
            _settings.Remove(settings);
        }
        var readStatus = _readStatuses.FirstOrDefault(rs => rs.UserId == userId);
        if (readStatus is not null)
        {
            _readStatuses.Remove(readStatus);
        }
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
