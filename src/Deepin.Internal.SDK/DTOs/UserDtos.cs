namespace Deepin.Internal.SDK.Models;

/// <summary>
/// Represents a user in the system
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public IEnumerable<UserCliamDto>? Claims { get; set; }
}
public class UserCliamDto
{
    public int Id { get; set; }
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
}

public record UserCliamRequest(string ClaimType, string ClaimValue);

/// <summary>
/// Request model for getting users by IDs
/// </summary>
public class BatchGetUsersRequest
{
    public List<Guid> Ids { get; set; } = new();
}

/// <summary>
/// Request model for searching users
/// </summary>
public class SearchUsersRequest : PagedQuery
{
}