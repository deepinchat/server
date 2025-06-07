using Deepin.Application.IntegrationEvents;

namespace Deepin.Application.Interfaces;
public interface IIntegrationEventHandler<in TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
{
    Task HandleAsync(TIntegrationEvent @event, CancellationToken cancellationToken);
}