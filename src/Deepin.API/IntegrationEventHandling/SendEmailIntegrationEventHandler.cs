using Deepin.Application.IntegrationEvents;
using Deepin.Application.Interfaces;
using Deepin.Domain.Emails;
using Deepin.Infrastructure.Configurations;
using Deepin.Infrastructure.EventBus;

namespace Deepin.API.IntegrationEventHandling;

public class SendEmailIntegrationEventHandler(
    ILogger<SendEmailIntegrationEventHandler> logger,
    SmtpOptions smtpOptions,
    IEmailSender emailSender,
    IEmailRepository emailRepository)
: IntegrationEventHandler<SendEmailIntegrationEvent>(logger)
{

    public async override Task HandleAsync(SendEmailIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var email = new Email(from: smtpOptions.FromAddress, to: string.Join(";", @event.To), subject: @event.Subject, body: @event.Body, isBodyHtml: true, cc: @event.CC == null ? null : string.Join(";", @event.CC));
        await emailRepository.AddAsync(email);
        await emailRepository.UnitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await emailRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await emailSender.SendAsync(smtpOptions.FromAddress, smtpOptions.FromDisplayName, @event.To, @event.Subject, @event.Body, true, @event.CC);
        }, cancellationToken);
    }
}
