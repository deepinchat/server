using Deepin.API.Models.Messages;
using Deepin.Application.DTOs.Chats;

namespace Deepin.API.Models.Chats;

public class ChatSummary
{
    public required ChatDto Chat { get; set; }
    public MessageSummary? LastMessage { get; set; }
    public int UnreadCount { get; set; }
}
