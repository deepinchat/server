using Deepin.Application.DTOs.Chats;
using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using Deepin.Chatting.Application.Constants;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record GetDirectChatCommand(Guid Id) : IRequest<DirectChatDto?>;
public record GetGroupChatCommand(Guid Id) : IRequest<GroupChatDto?>;
public class GetChatCommandHandler(ICacheManager cacheManager, IChatQueries chatQueries) :
IRequestHandler<GetDirectChatCommand, DirectChatDto?>,
IRequestHandler<GetGroupChatCommand, GroupChatDto?>
{
    public async Task<DirectChatDto?> Handle(GetDirectChatCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetChatByIdCacheKey(request.Id);
        return await cacheManager.GetOrSetAsync(cacheKey, () => chatQueries.GetDirectChatByIdAsync(request.Id, cancellationToken));
    }

    public async Task<GroupChatDto?> Handle(GetGroupChatCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetChatByIdCacheKey(request.Id);
        return await cacheManager.GetOrSetAsync(cacheKey, () => chatQueries.GetGroupChatByIdAsync(request.Id, cancellationToken));
    }
}