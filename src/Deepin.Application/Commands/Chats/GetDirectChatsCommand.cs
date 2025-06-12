using Deepin.Application.DTOs.Chats;
using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using Deepin.Chatting.Application.Constants;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record GetDirectChatsCommand(Guid UserId) : IRequest<IEnumerable<DirectChatDto>?>;
public class GetDirectChatsCommandHandler(ICacheManager cacheManager, IChatQueries chatQueries) : IRequestHandler<GetDirectChatsCommand, IEnumerable<DirectChatDto>?>
{
    public async Task<IEnumerable<DirectChatDto>?> Handle(GetDirectChatsCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetDirectChatsCacheKey(request.UserId);
        return await cacheManager.GetOrSetAsync(cacheKey, async () => await chatQueries.GetDirectChatsAsync(request.UserId, cancellationToken));
    }
}
