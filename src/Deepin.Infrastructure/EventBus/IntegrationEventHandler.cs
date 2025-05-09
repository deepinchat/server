using Deepin.Application.IntegrationEvents;
using Deepin.Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Deepin.Infrastructure.EventBus;

public abstract class IntegrationEventHandler<TIntegrationEvent>(ILogger logger) :
    IIntegrationEventHandler<TIntegrationEvent>,
    IConsumer<TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
{

    public async Task Consume(ConsumeContext<TIntegrationEvent> context)
    {
        try
        {
            logger.LogInformation("Handling integration event: {EventName}, details:{Details}", typeof(TIntegrationEvent).Name, context.Message);
            await HandleAsync(context.Message, context.CancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling integration event: {EventName}", typeof(TIntegrationEvent).Name);
        }
    }

    public abstract Task HandleAsync(TIntegrationEvent @event, CancellationToken cancellationToken);
}