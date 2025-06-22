using Deepin.Domain.ChatAggregate;
using Deepin.Domain.Exceptions;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record LeaveChatCommand(Guid Id, Guid UserId) : IRequest<Unit>;
public class LeaveChatCommandHandler(IGroupChatRepository groupChatRepository) : IRequestHandler<LeaveChatCommand, Unit>
{
    public async Task<Unit> Handle(LeaveChatCommand request, CancellationToken cancellationToken)
    {
        var groupChat = await groupChatRepository.FindByIdAsync(request.Id, cancellationToken);
        if (groupChat is null)
        {
            throw new EntityNotFoundException(nameof(GroupChat), request.Id);
        }

        groupChat.RemoveMember(request.UserId);
        await groupChatRepository.UpdateAsync(groupChat, cancellationToken);
        await groupChatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return Unit.Value;
    }
}