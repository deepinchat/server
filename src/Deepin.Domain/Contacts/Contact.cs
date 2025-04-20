namespace Deepin.Domain.Contacts;

public class Contact : Entity<Guid>, IAggregateRoot
{
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Company { get; private set; }
    public string? UserId { get; private set; }
    public string? Birthday { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Address { get; private set; }
    public string? Notes { get; private set; }
    public string CreatedBy { get; private set; }
    public bool IsDeleted { get; private set; }
    public bool IsBlocked { get; private set; }
    public bool IsStarred { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Contact()
    {
        CreatedBy = string.Empty;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public Contact(string createdBy, string? userId, string? firstName, string? lastName, string? company, string? birthday, string? email, string? phoneNumber, string? address, string? notes) : this()
    {
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
    }
    public void Update(string? userId, string? firstName, string? lastName, string? company, string? birthday, string? email, string? phoneNumber, string? address, string? notes)
    {
        FirstName = firstName;
        LastName = lastName;
        Company = company;
        UserId = userId;
        Birthday = birthday;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateUserId(string userId)
    {
        UserId = userId;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Block()
    {
        IsBlocked = true;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Unblock()
    {
        IsBlocked = false;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Star()
    {
        IsStarred = true;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Unstar()
    {
        IsStarred = false;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Restore()
    {
        IsDeleted = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
