using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public class UpdateChatReadStatusCommandHandler(IChatRepository chatRepository) : IRequestHandler<UpdateChatReadStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateChatReadStatusCommand request, CancellationToken cancellationToken)
    {
        await chatRepository.AddOrUpdateReadStatusAsync(
            request.ChatId,
            request.UserId,
            request.MessageId,
            cancellationToken
        );
        return true;
    }
}
