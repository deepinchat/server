using Deepin.Application.DTOs.Chats;
using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using Deepin.Chatting.Application.Constants;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record GetChatsCommand(Guid UserId) : IRequest<IEnumerable<ChatDto>?>;

public class GetChatsCommandHandler(ICacheManager cacheManager, IChatQueries chatQueries) : IRequestHandler<GetChatsCommand, IEnumerable<ChatDto>?>
{
    public async Task<IEnumerable<ChatDto>?> Handle(GetChatsCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetChatsCacheKey(request.UserId);
        return await cacheManager.GetOrSetAsync(cacheKey, () => chatQueries.GetChats(request.UserId, cancellationToken));
    }
}