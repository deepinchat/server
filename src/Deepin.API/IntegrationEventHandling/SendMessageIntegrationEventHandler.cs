using Deepin.Application.Commands.Messages;
using Deepin.Application.IntegrationEvents;
using Deepin.Application.Requests.Messages;
using Deepin.Infrastructure.EventBus;
using MediatR;

namespace Deepin.API.IntegrationEventHandling;

public class SendMessageIntegrationEventHandler(ILogger<SendMessageIntegrationEventHandler> logger, IMediator mediator)
: IntegrationEventHandler<SendMessageIntegrationEvent>(logger)
{
    public override async Task HandleAsync(SendMessageIntegrationEvent @event, CancellationToken cancellationToken)
    {
        try
        {
            var request = new MessageRequest(
                ChatId: @event.ChatId,
                Type: @event.Type,
                Content: @event.Content
            );
            await mediator.Send(new SendMessageCommand(request, @event.SenderId), cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling SendMessageIntegrationEvent for ChatId: {ChatId}", @event.ChatId);
        }
    }
}
