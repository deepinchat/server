using Deepin.Application.DTOs.Messages;
using Deepin.Domain.MessageAggregate;
using Newtonsoft.Json.Linq;

namespace Deepin.Application.Requests.Messages;

public record MessageRequest(
    Guid ChatId,
    MessageType Type,
    string? Content = null,
    Guid? ParentId = null,
    Guid? ReplyToId = null,
    IEnumerable<MessageAttachmentRequest>? Attachments = null,
    IEnumerable<MessageMentionDto>? Mentions = null,
    JObject? Metadata = null);
