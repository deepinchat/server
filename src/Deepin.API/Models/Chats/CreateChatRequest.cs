using Deepin.Domain.ChatAggregate;

namespace Deepin.API.Models.Chats;

public class CreateChatRequest
{
    public string Name { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Description { get; set; }
    public Guid? AvatarFileId { get; set; }
    public bool IsPublic { get; set; }
    public ChatType Type { get; set; }
}
