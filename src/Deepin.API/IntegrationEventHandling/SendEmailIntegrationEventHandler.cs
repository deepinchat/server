using Deepin.Application.IntegrationEvents;
using Deepin.Domain.Emails;
using Deepin.Infrastructure.Configurations;
using Deepin.Infrastructure.Data;
using Deepin.Infrastructure.EventBus;
using Deepin.Infrastructure.Services;
using MassTransit;

namespace Deepin.API.IntegrationEventHandling;

public class SendEmailIntegrationEventHandler(
    ILogger<SendEmailIntegrationEventHandler> logger,
    SmtpOptions smtpOptions,
    IEmailSender emailSender, NotificationDbContext db)
: IIntegrationEventHandler<SendEmailIntegrationEvent>
{
    private readonly ILogger<SendEmailIntegrationEventHandler> _logger = logger;
    private readonly SmtpOptions _smtpOptions = smtpOptions;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly NotificationDbContext _db = db;

    public async Task Consume(ConsumeContext<SendEmailIntegrationEvent> context)
    {
        try
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", context.MessageId, context.Message);

            var @event = context.Message;
            await _emailSender.SendAsync(_smtpOptions.FromAddress, _smtpOptions.FromDisplayName, @event.To, @event.Subject, @event.Body, true, @event.CC);
            var email = new Email(from: _smtpOptions.FromAddress, to: string.Join(";", @event.To), subject: @event.Subject, body: @event.Body, isBodyHtml: true, cc: @event.CC == null ? null : string.Join(";", @event.CC));
            _db.Emails.Add(email);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR Processing message: {Message}", ex.Message);
        }
    }
}
