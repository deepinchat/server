namespace Deepin.Application.IntegrationEvents;

public record MessageSentIntegrationEvent(Guid ChatId, Guid MessageId) : IntegrationEvent;