using Deepin.Application.IntegrationEvents;

namespace Deepin.Application.Interfaces;

public interface IEventBus
{
    Task PublishAsync<T>(T @event) where T : IntegrationEvent;
}