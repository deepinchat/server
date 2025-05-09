using Deepin.API.Hubs;
using Deepin.Application.Commands.Messages;
using Deepin.Application.IntegrationEvents;
using Deepin.Infrastructure.EventBus;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Deepin.Chatting.API.EventHandling;

public class PushMessageIntegrationEventHandler(
    IMediator mediator,
    ILogger<PushMessageIntegrationEventHandler> logger,
    IHubContext<ChatsHub> chatsHub) : IntegrationEventHandler<PushMessageIntegrationEvent>(logger)
{

    public override async Task HandleAsync(PushMessageIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var message = await mediator.Send(new GetMessageCommand(@event.MessageId), cancellationToken);
        if (message is null)
        {
            logger.LogWarning("Message with ID {MessageId} not found", @event.MessageId);
            return;
        }
        await chatsHub.Clients
              .Group(@event.ChatId.ToString())
              .SendAsync("ReceiveMessage", message, cancellationToken: cancellationToken)
              .ContinueWith(task =>
              {
                  if (task.IsFaulted)
                  {
                      logger.LogError(task.Exception, "Error sending message to chat {ChatId}", @event.ChatId);
                  }
              }, cancellationToken);
    }
}
