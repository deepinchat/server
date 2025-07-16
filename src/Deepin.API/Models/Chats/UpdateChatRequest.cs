namespace Deepin.API.Models.Chats;

public class UpdateChatRequest
{
    public string Name { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Description { get; set; }
    public Guid? AvatarFileId { get; set; }
    public bool IsPublic { get; set; }
}
