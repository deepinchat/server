namespace Deepin.Application.Interfaces;

public interface IEmailSender
{
    Task SendAsync(string from, string fromDisplayName, string[] to, string subject, string body, bool isBodyHtml = true, string[]? cc = null);
}
