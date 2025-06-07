namespace Deepin.SDK.Models;

/// <summary>
/// Represents a user in the system
/// </summary>
public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsOnline { get; set; }
    public DateTime? LastSeenAt { get; set; }
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
    public string Query { get; set; } = string.Empty;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 20;
}