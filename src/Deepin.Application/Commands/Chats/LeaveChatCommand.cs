using MediatR;

namespace Deepin.Application.Commands.Chats;

public record LeaveChatCommand(Guid Id, Guid UserId) : IRequest<bool>;