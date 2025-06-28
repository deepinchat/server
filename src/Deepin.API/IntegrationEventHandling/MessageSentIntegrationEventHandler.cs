using Deepin.API.Hubs;
using Deepin.Application.Commands.Messages;
using Deepin.Application.IntegrationEvents;
using Deepin.Infrastructure.EventBus;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Deepin.Chatting.API.EventHandling;

public class MessageSentIntegrationEventHandler(
    IMediator mediator,
    ILogger<MessageSentIntegrationEventHandler> logger,
    IHubContext<ChatsHub> chatsHub) : IntegrationEventHandler<MessageSentIntegrationEvent>(logger)
{

    public override async Task HandleAsync(MessageSentIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var message = await mediator.Send(new GetMessageCommand(@event.MessageId), cancellationToken);
        if (message is null)
        {
            logger.LogWarning("Message with ID {MessageId} not found", @event.MessageId);
            return;
        }
        await chatsHub.Clients
              .Group(@event.ChatId.ToString())
              .SendAsync("NewMessage", message, cancellationToken: cancellationToken)
              .ContinueWith(task =>
              {
                  if (task.IsFaulted)
                  {
                      logger.LogError(task.Exception, "Error sending message to chat {ChatId}", @event.ChatId);
                  }
              }, cancellationToken);
    }
}
