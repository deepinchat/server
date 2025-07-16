using Deepin.Application.Interfaces;
using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record UpdateChatReadStatusCommand(Guid ChatId, Guid MessageId) : IRequest<Unit>;
public class UpdateChatReadStatusCommandHandler(
    IUserContext userContext,
    IChatReadStatusRepository chatReadStatusRepository) : IRequestHandler<UpdateChatReadStatusCommand, Unit>
{
    public async Task<Unit> Handle(UpdateChatReadStatusCommand request, CancellationToken cancellationToken)
    {
        var chatReadStatus = await chatReadStatusRepository.GetByChatIdAsync(request.ChatId, userContext.UserId, cancellationToken);
        if (chatReadStatus is null)
        {
            chatReadStatus = new ChatReadStatus(request.ChatId, userContext.UserId, request.MessageId);
            await chatReadStatusRepository.AddAsync(chatReadStatus, cancellationToken);
        }
        else
        {
            chatReadStatus.ReadMessage(request.MessageId);
            await chatReadStatusRepository.UpdateAsync(chatReadStatus, cancellationToken);
        }
        // Save changes to the repository
        await chatReadStatusRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return Unit.Value;
    }
}
