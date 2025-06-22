using Deepin.Application.Queries;
using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record DeleteChatCommand(Guid Id) : IRequest<Unit>;
public class DeleteChatCommandHandler(
    IChatQueries chatQueries,
    IGroupChatRepository groupChatRepository,
    IDirectChatRepository directChatRepository) : IRequestHandler<DeleteChatCommand, Unit>
{
    public async Task<Unit> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        var chatType = await chatQueries.GetChatTypeAsync(request.Id, cancellationToken);
        if (chatType == ChatType.Group)
        {
            var groupChat = await groupChatRepository.FindByIdAsync(request.Id, cancellationToken);
            if (groupChat is null)
            {
                return Unit.Value;
            }

            await groupChatRepository.DeleteAsync(groupChat, cancellationToken);
            await groupChatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
        else if (chatType == ChatType.Direct)
        {
            var directChat = await directChatRepository.FindByIdAsync(request.Id, cancellationToken);
            if (directChat is null)
            {
                return Unit.Value;
            }

            await directChatRepository.DeleteAsync(directChat, cancellationToken);
            await directChatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
        return Unit.Value;
    }
}
