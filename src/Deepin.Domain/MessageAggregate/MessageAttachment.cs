namespace Deepin.Domain.MessageAggregate;

public class MessageAttachment : Entity<Guid>
{
    public AttachmentType Type { get; private set; }
    public Guid FileId { get; private set; }
    public string FileName { get; private set; }
    public long FileSize { get; private set; }
    public string ContentType { get; private set; }
    public int Order { get; private set; }
    public Guid? ThumbnailFileId { get; private set; }
    public string? Metadata { get; private set; }
    public MessageAttachment(AttachmentType type, Guid fileId, string fileName, long fileSize, string contentType, int order, Guid? thumbnailFileId = null, string? metadata = null)
    {
        Type = type;
        FileId = fileId;
        FileName = fileName;
        FileSize = fileSize;
        ContentType = contentType;
        Order = order;
        ThumbnailFileId = thumbnailFileId;
        Metadata = metadata;
    }
}
