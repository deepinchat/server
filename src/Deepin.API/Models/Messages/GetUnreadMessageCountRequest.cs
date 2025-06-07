namespace Deepin.API.Models.Messages;

public class GetUnreadMessageCountRequest
{
    public Guid ChatId { get; set; }
    public DateTimeOffset? LastReadAt { get; set; }
}
