namespace Deepin.Domain.ChatAggregate;

public class GroupChat : ChatBase
{
    public string Name { get; private set; }
    public string? UserName { get; private set; }
    public string? Description { get; private set; }
    public Guid? AvatarFileId { get; private set; }
    public bool IsPublic { get; private set; }
    public GroupChat() : base()
    {
        Name = string.Empty;
    }
    public GroupChat(Guid userId, string name, string? userName = null, string? description = null, Guid? avatarFileId = null, bool isPublic = false) : base(userId, ChatType.Group)
    {
        Name = name;
        UserName = userName;
        Description = description;
        AvatarFileId = avatarFileId;
        IsPublic = isPublic;
    }
    public void Update(string name, string? userName = null, string? description = null, Guid? avatarFileId = null, bool isPublic = false)
    {
        Name = name;
        UserName = userName;
        Description = description;
        AvatarFileId = avatarFileId;
        IsPublic = isPublic;
    }
}
