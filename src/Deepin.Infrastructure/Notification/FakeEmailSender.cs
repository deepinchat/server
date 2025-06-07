using Deepin.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepin.Infrastructure.Services;

public class FakeEmailSender(ILogger<FakeEmailSender> logger) : IEmailSender
{
    private readonly ILogger<FakeEmailSender> _logger = logger;
    public async Task SendAsync(string from, string fromDisplayName, string[] to, string subject, string body, bool isBodyHtml = true, string[]? cc = null)
    {
        _logger.LogInformation("Sending email from: {from},fromDisplayName:{fromDisplayName},to: {To}, subject: {Subject}, body: {Body}, isBodyHtml: {IsBodyHtml}, cc: {Cc}", from, fromDisplayName, to, subject, body, isBodyHtml, cc);
        await Task.CompletedTask;
    }
}

