using Deepin.Application.DTOs.Chats;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record CreateDirectChatCommand(Guid OwnerId, Guid[] Others) : IRequest<ChatDto>;
