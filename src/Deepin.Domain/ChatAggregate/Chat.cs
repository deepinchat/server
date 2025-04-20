using Deepin.Domain.Events;

namespace Deepin.Domain.ChatAggregate;
public class Chat : Entity<Guid>, IAggregateRoot
{
    private List<ChatMember> _members = [];
    private List<ChatReadStatus> _readStatuses = [];
    public ChatType Type { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public GroupInfo? GroupInfo { get; private set; }
    public IReadOnlyCollection<ChatMember> Members => _members;
    public IReadOnlyCollection<ChatReadStatus> ReadStatuses => _readStatuses;
    public Chat()
    {
        CreatedBy = string.Empty;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new ChatCreatedDomainEvent(this));
    }
    public Chat(ChatType type, string createdBy, GroupInfo? groupInfo = null) : this()
    {
        Type = type;
        CreatedBy = createdBy;
        GroupInfo = groupInfo;
        this.AddMember(new ChatMember(createdBy, ChatMemberRole.Owner));
    }
    public void UpdateGroupInfo(GroupInfo groupInfo)
    {
        GroupInfo = groupInfo;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new ChatGroupInfoUpdatedDomainEvent(this));
    }
    public void AddMember(ChatMember member)
    {
        if (_members.Any(x => x.UserId == member.UserId))
        {
            throw new InvalidOperationException("Member already exists in the chat.");
        }
        _members.Add(member);
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new ChatMemberAddedDomainEvent(this, member));
    }
    public void RemoveMember(string userId)
    {
        var member = _members.FirstOrDefault(x => x.UserId == userId);
        if (member is null)
        {
            throw new InvalidOperationException("Member does not exist in the chat.");
        }
        _members.Remove(member);
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new ChatMemberRemovedDomainEvent(this, member));
    }
    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new ChatDeletedDomainEvent(this));
    }
}
