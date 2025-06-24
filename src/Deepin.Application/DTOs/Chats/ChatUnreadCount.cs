using System;

namespace Deepin.Application.DTOs.Chats;

public class ChatUnreadCount
{
    public Guid ChatId { get; set; }
    public int UnreadCount { get; set; }
    public ChatUnreadCount(Guid chatId, int unreadCount)
    {
        ChatId = chatId;
        UnreadCount = unreadCount;
    }
}
