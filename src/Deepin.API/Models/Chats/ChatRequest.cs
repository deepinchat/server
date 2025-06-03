using Deepin.Domain.ChatAggregate;

namespace Deepin.API.Models.Chats;

public class ChatRequest
{
    public required string Name { get; set; }
    public string? UserName { get; set; }
    public string? Description { get; set; }
    public Guid? AvatarFileId { get; set; }
    public bool IsPublic { get; set; }
    public ChatType Type { get; set; }
}
