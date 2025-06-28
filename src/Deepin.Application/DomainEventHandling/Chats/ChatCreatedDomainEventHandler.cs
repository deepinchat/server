using Deepin.Application.Constants;
using Deepin.Application.IntegrationEvents;
using Deepin.Application.Interfaces;
using Deepin.Domain.Events;
using Deepin.Domain.MessageAggregate;
using MediatR;

namespace Deepin.Application.DomainEventHandling.Chats;

public class ChatCreatedDomainEventHandler(IEventBus eventBus) : INotificationHandler<ChatCreatedDomainEvent>
{
    public async Task Handle(ChatCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        var @event = new SendMessageIntegrationEvent(
            ChatId: notification.Chat.Id,
            Type: MessageType.System,
            Content: SystemMessageKeys.ChatCreated,
            SenderId: notification.Chat.CreatedBy
        );
        await eventBus.PublishAsync(@event, cancellationToken);
    }
}
