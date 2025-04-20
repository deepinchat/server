using Deepin.API.Hubs;
using Deepin.Application.IntegrationEvents;
using Deepin.Infrastructure.EventBus;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Deepin.Chatting.API.EventHandling;

public class SendMessageIntegrationEventHandler(ILogger<SendMessageIntegrationEventHandler> logger, IHubContext<ChatsHub> chatsHub) : IIntegrationEventHandler<SendMessageIntegrationEvent>
{
    private readonly ILogger<SendMessageIntegrationEventHandler> _logger = logger;
    private readonly IHubContext<ChatsHub> _chatsHub = chatsHub;
    public async Task Consume(ConsumeContext<SendMessageIntegrationEvent> context)
    {
        try
        {
            var chatMessage = context.Message;
            await _chatsHub.Clients.Group(chatMessage.ChatId).SendAsync("ReceiveMessage", chatMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending message to chat");
        }
    }
}
