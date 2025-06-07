using Deepin.Domain.MessageAggregate;
using Newtonsoft.Json.Linq;

namespace Deepin.Application.Requests.Messages;

public record MessageAttachmentRequest(
    AttachmentType Type,
    int Order,
    Guid FileId,
    string FileName,
    long FileSize,
    string ContentType,
    Guid? ThumbnailFileId,
    JObject? Metadata);