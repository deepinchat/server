using Deepin.Application.Models.Chats;
using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public class CreateChatCommand : IRequest<ChatDto>
{
    public required string Name { get; set; }
    public string? UserName { get; set; }
    public string? Description { get; set; }
    public string? AvatarFileId { get; set; }
    public bool IsPublic { get; set; }
    public ChatType Type { get; set; } 
}
