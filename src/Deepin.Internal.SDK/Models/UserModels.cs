namespace Deepin.Internal.SDK.Models;

/// <summary>
/// Represents a user in the system
/// </summary>
public class User
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
    public IEnumerable<UserCliam>? Claims { get; set; }
}
public class UserCliam
{
    public int Id { get; set; }
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
}

/// <summary>
/// Request model for getting users by IDs
/// </summary>
public class GetUsersByIdsRequest
{
    public List<int> UserIds { get; set; } = new();
}

/// <summary>
/// Request model for searching users
/// </summary>
public class SearchUsersRequest
{
    public string? Query { get; set; } = string.Empty;
    public int Limit { get; set; } = 20;
    public int Offset { get; set; } = 0;
}