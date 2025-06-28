using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record AddChatMessageCommand(Guid ChatId, Guid MessageId, string MessageType, DateTimeOffset SentAt, Guid? SenderId = null) : IRequest<Unit>;
public class AddChatMessageCommandHandler(IChatMessageRepository chatMessageRepository) : IRequestHandler<AddChatMessageCommand, Unit>
{
    public async Task<Unit> Handle(AddChatMessageCommand request, CancellationToken cancellationToken)
    {
        var chatMessage = new ChatMessage(
            chatId: request.ChatId,
            type: Enum.Parse<ChatMessageType>(request.MessageType, true),
            messageId: request.MessageId,
            sentAt: request.SentAt,
            senderId: request.SenderId);
        await chatMessageRepository.AddAsync(chatMessage, cancellationToken);
        await chatMessageRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}