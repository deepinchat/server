using Deepin.Domain.ChatAggregate;
using Deepin.Domain.Exceptions;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public class JoinChatCommandHandler(IChatRepository chatRepository) : IRequestHandler<JoinChatCommand, bool>
{
    public async Task<bool> Handle(JoinChatCommand request, CancellationToken cancellationToken)
    {
        var chat = await chatRepository.FindByIdAsync(request.Id, cancellationToken);
        if (chat is null)
        {
            throw new DomainException($"Chat with Id {request.Id} was not found");
        }
        chat.AddMember(new ChatMember(request.UserId, ChatMemberRole.Member));
        await chatRepository.UpdateAsync(chat);
        await chatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}