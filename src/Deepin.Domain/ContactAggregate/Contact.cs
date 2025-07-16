namespace Deepin.Domain.ContactAggregate;

public class Contact : Entity<Guid>, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public string? Name { get; private set; }
    public string? Notes { get; private set; }
    public bool IsStarred { get; private set; }
    public bool IsBlocked { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public Contact()
    {
    }
    public Contact(Guid ownerId, Guid userId, string? name, string? notes = null, bool isStarred = false, bool isBlocked = false) : this()
    {
        CreatedBy = ownerId;
        UserId = userId;
        Notes = notes;
        Name = name;
        IsStarred = isStarred;
        IsBlocked = isBlocked;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Update(string? name, string? notes = null)
    {
        Name = name;
        Notes = notes;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Block()
    {
        IsBlocked = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Unblock()
    {
        IsBlocked = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Star()
    {
        IsStarred = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Unstar()
    {
        IsStarred = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
