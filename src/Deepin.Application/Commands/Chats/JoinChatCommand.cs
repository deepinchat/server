using MediatR;

namespace Deepin.Application.Commands.Chats;

public record JoinChatCommand(Guid Id, Guid UserId) : IRequest<bool>;