using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public class DeleteChatCommandHandler(IChatRepository chatRepository) : IRequestHandler<DeleteChatCommand, bool>
{
    private readonly IChatRepository _chatRepository = chatRepository;
    public async Task<bool> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        var chat = await _chatRepository.FindByIdAsync(request.Id, cancellationToken);
        if (chat is null)
        {
            return false;
        }

        chat.Delete();
        await _chatRepository.UpdateAsync(chat);
        return await _chatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }

}
