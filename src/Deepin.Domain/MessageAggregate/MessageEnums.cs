namespace Deepin.Domain.MessageAggregate;

public enum MessageType
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

public enum MentionType
{
    User,
    All
}

public enum AttachmentType
{
    Image,
    Video,
    Audio,
    Document,
    File
}