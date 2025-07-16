namespace Deepin.Application.DTOs.Contacts;

public record CreateContactRequest(Guid UserId, string? Name, string? Notes);