namespace Deepin.Domain.MessageAggregate;

public class MessageMention : Entity<Guid>
{
    public MentionType Type { get; set; }
    public Guid UserId { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public MessageMention(MentionType type, Guid userId, int startIndex, int endIndex)
    {
        Type = type;
        UserId = userId;
        StartIndex = startIndex;
        EndIndex = endIndex;
    }
}
