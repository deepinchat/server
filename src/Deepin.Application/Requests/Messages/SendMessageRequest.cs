using Deepin.Application.DTOs.Messages;
using Deepin.Domain.MessageAggregate;
using Newtonsoft.Json.Linq;

namespace Deepin.Application.Requests.Messages;

public record SendMessageRequest(
    MessageType Type,
    Guid ChatId,
    Guid? ParentId,
    Guid? ReplyToId,
    string? Text,
    IEnumerable<MessageAttachmentRequest>? Attachments,
    IEnumerable<MessageMention>? Mentions,
    JObject? Metadata);
