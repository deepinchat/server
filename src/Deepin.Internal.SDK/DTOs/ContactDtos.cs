using System;

namespace Deepin.Internal.SDK.DTOs;

public record ContactRequest(
    Guid? UserId,
    string? Name,
    string? FirstName,
    string? LastName,
    string? Company,
    string? Birthday,
    string? Email,
    string? PhoneNumber,
    string? Address,
    string? Notes);

public class ContactDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Company { get; set; }
    public string? Birthday { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsStarred { get; set; }
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? UserId { get; set; }
}

public class SearchContactRequest : PagedQuery;