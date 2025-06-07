using Deepin.Application.DTOs.Chats;
using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using Deepin.Chatting.Application.Constants;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record GetChatCommand(Guid Id) : IRequest<ChatDto?>;
public class GetChatCommandHandler(ICacheManager cacheManager, IChatQueries chatQueries) : IRequestHandler<GetChatCommand, ChatDto?>
{
    public async Task<ChatDto?> Handle(GetChatCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetChatByIdCacheKey(request.Id);
        return await cacheManager.GetOrSetAsync(cacheKey, () => chatQueries.GetChat(request.Id, cancellationToken));
    }
}