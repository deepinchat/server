using Deepin.Application.Models.Chats;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record CreateDirectChatCommand(string[] UserIds) : IRequest<ChatDto>;
