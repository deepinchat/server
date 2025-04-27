using Deepin.API.Hubs;
using Deepin.Application.IntegrationEvents;
using Deepin.Infrastructure.EventBus;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Deepin.Chatting.API.EventHandling;

public class SendMessageIntegrationEventHandler(ILogger<SendMessageIntegrationEventHandler> logger, IHubContext<ChatsHub> chatsHub)
: IntegrationEventHandler<SendMessageIntegrationEvent>(logger)
{

    public override async Task HandleAsync(SendMessageIntegrationEvent @event, CancellationToken cancellationToken)
    {
        await chatsHub.Clients.Group(@event.ChatId).SendAsync("ReceiveMessage", @event);
    }
}
