namespace Deepin.Domain.Emails;

public class Email
{
    public long Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string? CC { get; set; }
    public bool IsBodyHtml { get; set; }
    public DateTime CreatedAt { get; set; }
    public Email()
    {
        From = string.Empty;
        To = string.Empty;
        CC = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
    }
    public Email(string from, string to, string subject, string body, bool isBodyHtml = true, string? cc = null) : this()
    {
        From = from ?? "test";
        To = to;
        Subject = subject;
        Body = body;
        IsBodyHtml = isBodyHtml;
        CC = cc;
    }
}