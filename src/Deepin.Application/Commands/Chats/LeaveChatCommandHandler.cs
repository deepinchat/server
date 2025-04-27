using Deepin.Domain.ChatAggregate;
using Deepin.Domain.Exceptions;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public class LeaveChatCommandHandler(IChatRepository chatRepository) : IRequestHandler<LeaveChatCommand, bool>
{
    private readonly IChatRepository _chatRepository = chatRepository;
    public async Task<bool> Handle(LeaveChatCommand request, CancellationToken cancellationToken)
    {
        var chat = await _chatRepository.FindByIdAsync(request.Id, cancellationToken);
        if (chat is null)
        {
            throw new DomainException($"Chat with Id {request.Id} was not found");
        }

        chat.RemoveMember(request.UserId);
        await _chatRepository.UpdateAsync(chat);
        return await _chatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}