using Deepin.Application.DTOs.Chats;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public class UpdateChatCommand : IRequest<ChatDto>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? UserName { get; set; }
    public string? Description { get; set; }
    public Guid? AvatarFileId { get; set; }
    public bool IsPublic { get; set; }
}

