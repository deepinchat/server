namespace Deepin.Application.DTOs.Contacts;

public record ContactRequest(Guid? UserId, string? Name, string? FirstName, string? LastName, string? Company, string? Birthday, string? Email, string? PhoneNumber, string? Address, string? Notes);