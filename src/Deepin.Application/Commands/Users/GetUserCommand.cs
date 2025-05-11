using Deepin.Application.DTOs.Users;
using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using Deepin.Chatting.Application.Constants;
using MediatR;

namespace Deepin.Application.Commands.Users;

public record GetUserCommand(Guid Id) : IRequest<UserDto?>;

public class GetUserCommandHandler(ICacheManager cacheManager, IUserQueries userQueries) : IRequestHandler<GetUserCommand, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        var user = await cacheManager.GetOrSetAsync(CacheKeys.GetUserByIdCacheKey(request.Id), async () =>
        {
            return await userQueries.GetUserAsync(request.Id, cancellationToken);
        });

        return user;
    }
}