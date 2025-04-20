namespace Deepin.Domain.ChatAggregate;
public class GroupInfo : ValueObject
{
    public string Name { get; set; }
    public string? UserName { get; set; }
    public string? Description { get; set; }
    public string? AvatarFileId { get; set; }
    public bool IsPublic { get; set; }
    public GroupInfo()
    {
        Name = string.Empty;
    }
    public GroupInfo(string name, string? userName = null, string? description = null, string? avatarFileId = null, bool isPublic = false) : this()
    {
        Name = name;
        UserName = userName;
        Description = description;
        AvatarFileId = avatarFileId;
        IsPublic = isPublic;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return UserName ?? string.Empty;
        yield return Description ?? string.Empty;
        yield return AvatarFileId ?? string.Empty;
        yield return IsPublic;
    }
}
