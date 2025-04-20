namespace Deepin.Infrastructure.Configurations;

public class SmtpOptions
{
    public required string Host { get; set; }
    public int Port { get; set; }
    public required string ReplyTo { get; set; }
    public required string Password { get; set; }
    public required string FromAddress { get; set; }
    public required string FromDisplayName { get; set; }
    public bool IsEnabled => !string.IsNullOrEmpty(Host) && Port > 0 && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(FromDisplayName) && !string.IsNullOrEmpty(FromAddress);

}
