using System.Security.Claims;
using Deepin.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Deepin.API.Hubs;

[Authorize]
public class ChatsHub(IChatQueries chatQueries) : Hub
{
    private readonly IChatQueries _chatQueries = chatQueries;
    private string? _userId = null;
    public Guid? UserId
    {
        get
        {
            if (string.IsNullOrEmpty(_userId))
            {
                _userId = Context.User?.FindFirst("sub")?.Value;
            }
            if (string.IsNullOrEmpty(_userId))
            {
                _userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            return string.IsNullOrEmpty(_userId) ? null : Guid.Parse(_userId);
        }
    }
    public override async Task OnConnectedAsync()
    {
        var chats = await _chatQueries.GetGroupChatsAsync(UserId ?? throw new ArgumentNullException(nameof(UserId)));
        foreach (var chat in chats)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
        }
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var chats = await _chatQueries.GetGroupChatsAsync(UserId ?? throw new ArgumentNullException(nameof(UserId)));
        foreach (var chat in chats)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chat.Id.ToString());
        }
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// 标记消息为已读
    /// </summary>
    public async Task MarkAsRead(Guid chatId, Guid messageId)
    {
        if (!UserId.HasValue) return;

        // 通知该聊天的其他成员，某用户已读了消息
        await Clients.GroupExcept(chatId.ToString(), Context.ConnectionId)
            .SendAsync("UserReadMessage", new { 
                ChatId = chatId, 
                UserId = UserId.Value, 
                MessageId = messageId,
                ReadAt = DateTimeOffset.UtcNow 
            });
    }

    /// <summary>
    /// 用户开始输入
    /// </summary>
    public async Task StartTyping(Guid chatId)
    {
        if (!UserId.HasValue) return;

        await Clients.GroupExcept(chatId.ToString(), Context.ConnectionId)
            .SendAsync("UserStartTyping", new { ChatId = chatId, UserId = UserId.Value });
    }

    /// <summary>
    /// 用户停止输入
    /// </summary>
    public async Task StopTyping(Guid chatId)
    {
        if (!UserId.HasValue) return;

        await Clients.GroupExcept(chatId.ToString(), Context.ConnectionId)
            .SendAsync("UserStopTyping", new { ChatId = chatId, UserId = UserId.Value });
    }
}
