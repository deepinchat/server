namespace Deepin.Application.DTOs.Contacts;

public class ContactDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string? Name { get; init; }
    public string? Notes { get; init; }
    public bool IsBlocked { get; init; }
    public bool IsStarred { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
