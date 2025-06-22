using Deepin.Application.IntegrationEvents;
using Deepin.Application.Queries;
using Deepin.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using Deepin.Application.Interfaces;

namespace Deepin.API.IntegrationEventHandling;

public class MessageSentIntegrationEventHandler(
    IHubContext<ChatsHub> hubContext,
    IChatQueries chatQueries) : IIntegrationEventHandler<PushMessageIntegrationEvent>
{
    public async Task HandleAsync(PushMessageIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        // 获取聊天成员
        var chatMembers = await chatQueries.GetChatMembers(@event.ChatId, 0, 1000, cancellationToken);

        // // 为每个成员计算未读消息数量（排除发送者）
        // foreach (var member in chatMembers.Items.Where(m => m.UserId != GetMessageSender(@event.MessageId)))
        // {
        //     var unreadCount = await unreadMessageQueries.GetUnreadMessageCountAsync(
        //         @event.ChatId,
        //         member.UserId,
        //         cancellationToken);

        //     // 发送未读数量更新到该用户
        //     await hubContext.Clients.Group($"user-{member.UserId}")
        //         .SendAsync("UnreadCountUpdated", new
        //         {
        //             ChatId = @event.ChatId,
        //             UnreadCount = unreadCount,
        //             MessageId = @event.MessageId
        //         }, cancellationToken);
        // }

        // 发送新消息通知到聊天组
        await hubContext.Clients.Group(@event.ChatId.ToString())
            .SendAsync("NewMessage", new
            {
                ChatId = @event.ChatId,
                MessageId = @event.MessageId
            }, cancellationToken);
    }
}
