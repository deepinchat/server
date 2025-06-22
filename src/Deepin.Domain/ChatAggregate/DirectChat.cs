namespace Deepin.Domain.ChatAggregate;

public class DirectChat : ChatBase
{
    public DirectChat() : base()
    {
    }
    public DirectChat(Guid currentUserId, Guid otherUserId) : base(currentUserId)
    {
        base.AddMember(otherUserId, ChatMemberRole.Owner);
    }
    public Guid GetOtherUserId(Guid currentUserId)
    {
        if (Members.Count != 2)
            throw new InvalidOperationException("Direct chat must have exactly two members.");

        return Members.First(m => m.UserId != currentUserId).UserId;
    }
}
