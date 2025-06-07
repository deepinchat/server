namespace Deepin.Application.DTOs.Users;

public class UserCliamDto
{
    public int Id { get; set; } = default!;
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
}
