using Deepin.Application.DTOs.Chats;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record CreateDirectChatCommand(Guid[] UserIds) : IRequest<ChatDto>;
