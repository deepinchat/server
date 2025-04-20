using MediatR;

namespace Deepin.Application.Commands.Chats;

public record UpdateChatReadStatusCommand(Guid ChatId, string MessageId) : IRequest<bool>;
