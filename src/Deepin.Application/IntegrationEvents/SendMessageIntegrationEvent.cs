using Deepin.Domain.MessageAggregate;

namespace Deepin.Application.IntegrationEvents;

public record SendMessageIntegrationEvent(Guid ChatId, MessageType Type, string? Content = null, Guid? SenderId = null) : IntegrationEvent;