using Deepin.API.Extensions;
using Deepin.API.Models.Chats;
using Deepin.API.Models.Messages;
using Deepin.Application.Commands.Messages;
using Deepin.Application.Commands.Users;
using Deepin.Application.DTOs;
using Deepin.Application.Queries;
using Deepin.Application.Requests.Messages;
using MediatR;

namespace Deepin.API.Services;

public interface IChatService
{
    Task<ChatSummary?> GetChat(Guid id, Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<ChatSummary>> GetChats(Guid userId, CancellationToken cancellationToken);
    Task<MessageSummary?> GetMessage(Guid id, CancellationToken cancellationToken);
    Task<IPagedResult<MessageSummary>> SearchMessages(SearchMessageRequest request, CancellationToken cancellationToken);
}

public class ChatService(IMediator mediator, IChatQueries chatQueries, IMessageQueries messageQueries) : IChatService
{
    public async Task<ChatSummary?> GetChat(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var chat = await chatQueries.GetChatById(id, cancellationToken);
        if (chat is null)
        {
            return null;
        }
        var chatSummary = new ChatSummary
        {
            Chat = chat
        };
        var lastMessageId = await messageQueries.GetLastIdAsync(id, cancellationToken);
        if (lastMessageId != null)
        {
            var lastMessage = await GetMessage(lastMessageId.Value, cancellationToken);
            chatSummary.LastMessage = lastMessage;
        }
        var lastReadStatus = await chatQueries.GetChatReadStatusAsync(id, userId, cancellationToken);
        chatSummary.UnreadCount = await messageQueries.GetUnreadCountAsync(id, lastReadStatus?.LastReadAt, cancellationToken);
        return chatSummary;
    }
    public async Task<IEnumerable<ChatSummary>> GetChats(Guid userId, CancellationToken cancellationToken)
    {
        var chats = await chatQueries.GetChats(userId, cancellationToken);
        if (chats is null || !chats.Any())
        {
            return [];
        }
        var chatReadStaueses = await chatQueries.GetChatReadStatusesAsync(userId, cancellationToken);
        var lastMessageIds = await messageQueries.GetLastIdsAsync(chats.Select(c => c.Id).ToArray(), cancellationToken);
        var chatSummaries = new List<ChatSummary>();
        foreach (var chat in chats)
        {
            var chatSummary = new ChatSummary
            {
                Chat = chat
            };
            if (lastMessageIds.Any(x => x.Key == chat.Id))
            {
                var lastMessage = await GetMessage(lastMessageIds.First(x => x.Key == chat.Id).Value, cancellationToken);
                chatSummary.LastMessage = lastMessage;
            }
            var lastReadStatus = chatReadStaueses.FirstOrDefault(x => x.ChatId == chat.Id);
            chatSummary.UnreadCount = await messageQueries.GetUnreadCountAsync(chat.Id, lastReadStatus?.LastReadAt, cancellationToken);
            chatSummaries.Add(chatSummary);
        }
        return chatSummaries;
    }

    public async Task<MessageSummary?> GetMessage(Guid id, CancellationToken cancellationToken)
    {
        var message = await mediator.Send(new GetMessageCommand(id), cancellationToken);
        if (message is null)
        {
            return null;
        }
        var user = await mediator.Send(new GetUserCommand(message.UserId), cancellationToken);
        return new MessageSummary
        {
            Message = message,
            Sender = user?.ToUserProfile(),
        };
    }
    public async Task<IPagedResult<MessageSummary>> SearchMessages(SearchMessageRequest request, CancellationToken cancellationToken)
    {
        var messages = await mediator.Send(new SearchMessageCommand(request), cancellationToken);

        if (messages is null || !messages.Items.Any())
        {
            return new PagedResult<MessageSummary>([], request.Offset, request.Limit, 0);
        }
        var messageSummaries = new List<MessageSummary>();
        foreach (var message in messages.Items)
        {
            var user = await mediator.Send(new GetUserCommand(message.UserId), cancellationToken);
            messageSummaries.Add(new MessageSummary
            {
                Message = message,
                Sender = user?.ToUserProfile(),
            });
        }
        return new PagedResult<MessageSummary>(messageSummaries, request.Offset, request.Limit, messages.TotalCount);
    }

}
