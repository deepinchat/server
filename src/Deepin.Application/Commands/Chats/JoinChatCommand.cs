using MediatR;

namespace Deepin.Application.Commands.Chats;

public record JoinChatCommand(Guid Id, string UserId) : IRequest<bool>;