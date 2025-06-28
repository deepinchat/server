using Deepin.Domain.MessageAggregate;

namespace Deepin.Application.DTOs.Messages;

public class MessageDto
{
    public Guid Id { get; set; }
    public MessageType Type { get; set; }
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? ReplyToId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string? Content { get; set; }
    public string? Metadata { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsRead { get; set; }
    public bool IsEdited { get; set; }
    public bool IsPinned { get; set; }
    public List<MessageAttachmentDto> Attachments { get; set; } = [];
    public List<MessageMentionDto> Mentions { get; set; } = [];
}
