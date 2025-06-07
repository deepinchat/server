namespace Deepin.Application.IntegrationEvents;
public record PushMessageIntegrationEvent(Guid ChatId, Guid MessageId) : IntegrationEvent;