
using MassTransit;

namespace Deepin.Infrastructure.EventBus;
public interface IIntegrationEventHandler<in TIntegrationEvent> : IConsumer<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{ }