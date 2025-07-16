namespace Deepin.Domain.ChatAggregate;

public enum ChatType
{
    Direct,
    Group,
    Channel
}
public enum ChatMemberRole
{
    Owner,
    Admin,
    Member
}

public enum ChatNotificationLevel
{
    All,
    MentionsOnly,
    None
}

public enum ChatJoinRequestStatus
{
    Pending,
    Approved,
    Rejected,
    Cancelled
}
public enum ChatMessageType
{
    Text,
    Image,
    Video,
    Audio,
    Document,
    File,
    Location,
    Sticker,
    Contact,
    Poll,
    System
}