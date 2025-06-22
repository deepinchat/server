using Deepin.Application.DTOs.Chats;
using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using Deepin.Chatting.Application.Constants;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record GetDirectChatsCommand(Guid UserId) : IRequest<IEnumerable<DirectChatDto>?>;
public record GetGroupChatsCommand(Guid UserId) : IRequest<IEnumerable<GroupChatDto>?>;

public class GetChatsCommandHandler(ICacheManager cacheManager, IChatQueries chatQueries) :
IRequestHandler<GetDirectChatsCommand, IEnumerable<DirectChatDto>?>,
IRequestHandler<GetGroupChatsCommand, IEnumerable<GroupChatDto>?>
{
    public async Task<IEnumerable<DirectChatDto>?> Handle(GetDirectChatsCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetDirectChatsCacheKey(request.UserId);
        return await cacheManager.GetOrSetAsync(cacheKey, () => chatQueries.GetDirectChatsAsync(request.UserId, cancellationToken));
    }

    public async Task<IEnumerable<GroupChatDto>?> Handle(GetGroupChatsCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetMessageByIdCacheKey(request.UserId);
        return await cacheManager.GetOrSetAsync(cacheKey, () => chatQueries.GetGroupChatsAsync(request.UserId, cancellationToken));
    }
}