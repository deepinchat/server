namespace Deepin.Application.DTOs.Chats;

public class ChatUnreadCount
{
    public Guid ChatId { get; set; }
    public long UnreadCount { get; set; }
    public ChatUnreadCount(Guid chatId, long unreadCount)
    {
        ChatId = chatId;
        UnreadCount = unreadCount;
    }
}
