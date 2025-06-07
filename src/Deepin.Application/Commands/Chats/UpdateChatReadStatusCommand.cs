using MediatR;

namespace Deepin.Application.Commands.Chats;

public record UpdateChatReadStatusCommand(Guid ChatId, Guid UserId, Guid MessageId) : IRequest<bool>;
