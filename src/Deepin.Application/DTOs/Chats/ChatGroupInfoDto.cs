namespace Deepin.Application.DTOs.Chats;

public class ChatGroupInfoDto
{
    public string Name { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Description { get; set; }
    public string? AvatarFileId { get; set; }
    public bool IsPublic { get; set; }

}
