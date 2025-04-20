using System.Net.Mail;
using Deepin.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;

namespace Deepin.Infrastructure.Services;

public class SmtpEmailSender(ILogger<SmtpEmailSender> logger, SmtpOptions smtpOptions) : IEmailSender
{
    private readonly ILogger<SmtpEmailSender> _logger = logger;
    private readonly SmtpOptions _smtpOptions = smtpOptions;
    public async Task SendAsync(string from, string fromDisplayName, string[] to, string subject, string body, bool isBodyHtml = true, string[]? cc = null)
    {
        MailMessage mailMessage = new MailMessage();
        foreach (var destination in to)
        {
            mailMessage.To.Add(new MailAddress(destination));
        }
        mailMessage.From = new MailAddress(from, fromDisplayName);
        if (cc != null)
        {
            foreach (var destination in cc)
            {
                mailMessage.CC.Add(new MailAddress(destination));
            }
        }
        if (!string.IsNullOrEmpty(_smtpOptions.ReplyTo))
        {
            mailMessage.ReplyToList.Add(new MailAddress(_smtpOptions.ReplyTo));
        }
        mailMessage.Subject = subject;

        mailMessage.Body = body;
        mailMessage.IsBodyHtml = isBodyHtml;

        // add attachments
        // string file = "D:\\1.txt";
        // Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
        // mailMessage.Attachments.Add(data); 

        SmtpClient smtpClient = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port);
        smtpClient.EnableSsl = true;

        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(_smtpOptions.FromAddress, _smtpOptions.Password);
        smtpClient.Credentials = credentials;
        await smtpClient.SendMailAsync(mailMessage);
    }
}
