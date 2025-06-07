using Deepin.Application.IntegrationEvents;
using Deepin.Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Deepin.Infrastructure.EventBus;

public class IntegrationEventBus(
    ILogger<IntegrationEventBus> logger,
    IPublishEndpoint publishEndpoint) : IEventBus
{
    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IntegrationEvent
    {
        try
        {
            logger.LogInformation("Publishing event: {Event},{EventData}", @event.GetType().Name, @event);
            // logger.LogDebug("Event data: {EventData}", @event);
            await publishEndpoint.Publish(@event, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error publishing event: {Event}", @event.GetType().Name);
            throw;
        }
    }
}
