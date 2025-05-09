using Deepin.Domain.MessageAggregate;

namespace Deepin.Application.DTOs.Messages;

public class MessageAttachmentDto
{
    public Guid Id { get; set; }
    public AttachmentType Type { get; set; }
    public Guid FileId { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public string ContentType { get; set; }
    public int Order { get; set; }
    public Guid? ThumbnailFileId { get; set; }
    public string? Metadata { get; set; }
}
