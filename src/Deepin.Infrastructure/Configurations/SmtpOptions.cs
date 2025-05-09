namespace Deepin.Infrastructure.Configurations;

public class SmtpOptions
{
    public SmtpOptions()
    {
        Port = 25;
        Host = string.Empty;
        ReplyTo = string.Empty;
        Password = string.Empty;
        FromAddress = string.Empty;
        FromDisplayName = string.Empty;
    }
    public string Host { get; set; }
    public int Port { get; set; }
    public string ReplyTo { get; set; }
    public string Password { get; set; }
    public string FromAddress { get; set; }
    public string FromDisplayName { get; set; }
    public bool IsEnabled => !string.IsNullOrEmpty(Host) && Port > 0 && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(FromDisplayName) && !string.IsNullOrEmpty(FromAddress);

}
