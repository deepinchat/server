using Deepin.Application.Constants;
using Deepin.Application.IntegrationEvents;
using Deepin.Application.Interfaces;
using Deepin.Chatting.Application.Constants;
using Deepin.Domain.ChatAggregate;
using Deepin.Domain.Events;
using Deepin.Domain.MessageAggregate;
using MediatR;

namespace Deepin.Application.DomainEventHandling.Chats;

public class ChatCreatedDomainEventHandler(
    IEventBus eventBus,
    ICacheManager cacheManager,
    IUserContext userContext) : INotificationHandler<ChatCreatedDomainEvent>
{
    public async Task Handle(ChatCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }
        var cacheKey = notification.Type switch
        {
            ChatType.Direct => CacheKeys.GetDirectChatsCacheKey(userContext.UserId),
            ChatType.Group => CacheKeys.GetGroupChatsCacheKey(userContext.UserId),
            _ => throw new ArgumentOutOfRangeException(nameof(notification.Type), "Unsupported chat type")
        };
        await cacheManager.RemoveAsync(cacheKey);

        var @event = new SendMessageIntegrationEvent(
            ChatId: notification.Chat.Id,
            Type: MessageType.System,
            Content: SystemMessageKeys.ChatCreated,
            SenderId: notification.Chat.CreatedBy
        );
        await eventBus.PublishAsync(@event, cancellationToken);
    }
}
