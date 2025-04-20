using MediatR;

namespace Deepin.Application.Commands.Chats;

public record DeleteChatCommand(Guid Id) : IRequest<bool>;