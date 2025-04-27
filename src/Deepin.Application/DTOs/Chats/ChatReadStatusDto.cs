namespace Deepin.Application.DTOs.Chats;

public class ChatReadStatusDto
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public required string LastReadMessageId { get; set; }
    public DateTimeOffset LastReadAt { get; set; }
}
