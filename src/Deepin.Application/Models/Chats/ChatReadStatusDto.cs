namespace Deepin.Application.Models.Chats;

public class ChatReadStatusDto
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public required string LastReadMessageId { get; set; }
    public DateTime LastReadAt { get; set; }
}
