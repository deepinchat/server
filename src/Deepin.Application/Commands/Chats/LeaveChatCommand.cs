using MediatR;

namespace Deepin.Application.Commands.Chats;

public record LeaveChatCommand(Guid Id, string UserId) : IRequest<bool>;