using Deepin.Infrastructure.EventBus;

namespace Deepin.Application.IntegrationEvents;
public record SendMessageIntegrationEvent(string ChatId, string MessageId) : IntegrationEvent;