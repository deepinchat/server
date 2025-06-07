using Deepin.Domain.MessageAggregate;

namespace Deepin.Application.DTOs.Messages;

public class MessageMentionDto
{
    public MentionType Type { get; set; }
    public Guid? UserId { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
}
