using Deepin.Application.DTOs.Messages;
using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using Deepin.Chatting.Application.Constants;
using Deepin.Domain.Exceptions;
using Deepin.Domain.MessageAggregate;
using MediatR;

namespace Deepin.Application.Commands.Messages;

public class GetMessageCommandHandler(
    ICacheManager cacheManager,
    IMessageQueries messageQueries) : IRequestHandler<GetMessageCommand, MessageDto?>
{
    public async Task<MessageDto?> Handle(GetMessageCommand request, CancellationToken cancellationToken)
    {
        return await cacheManager.GetOrSetAsync(
            CacheKeys.GetMessageByIdCacheKey(request.Id),
            async () =>
            {
                var message = await messageQueries.GetMessageAsync(request.Id, cancellationToken) ?? throw new EntityNotFoundException(
                        nameof(Message),
                        request.Id);
                return message;
            },
            TimeSpan.FromMinutes(5));
    }
}
