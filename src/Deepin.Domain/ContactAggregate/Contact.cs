namespace Deepin.Domain.ContactAggregate;

public class Contact : Entity<Guid>, IAggregateRoot
{
    public string? Name { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Company { get; private set; }
    public Guid? UserId { get; private set; }
    public string? Birthday { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Address { get; private set; }
    public string? Notes { get; private set; }
    public bool IsDeleted { get; private set; }
    public bool IsBlocked { get; private set; }
    public bool IsStarred { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public Contact()
    {
    }
    public Contact(Guid createdBy, Guid? userId, string? name, string? firstName, string? lastName, string? company, string? birthday, string? email, string? phoneNumber, string? address, string? notes) : this()
    {
        if (createdBy == Guid.Empty)
        {
            throw new ArgumentException("createdBy cannot be empty.", nameof(createdBy));
        }
        CreatedBy = createdBy;
        FirstName = firstName;
        LastName = lastName;
        Company = company;
        UserId = userId;
        Birthday = birthday;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        Notes = notes;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        Name = name ?? (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName) ? null : $"{firstName} {lastName}".Trim());
    }
    public void Update(string? name, string? firstName, string? lastName, string? company, string? birthday, string? email, string? phoneNumber, string? address, string? notes)
    {
        FirstName = firstName;
        LastName = lastName;
        Company = company;
        Birthday = birthday;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        Notes = notes;
        UpdatedAt = DateTimeOffset.UtcNow;
        Name = name ?? (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName) ? null : $"{firstName} {lastName}".Trim());
    }
    public void UpdateUserId(Guid userId)
    {
        UserId = userId;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Delete()
    {
        IsDeleted = true;
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
    public void Restore()
    {
        IsDeleted = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
