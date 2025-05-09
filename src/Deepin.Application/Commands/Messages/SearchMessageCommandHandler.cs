using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Messages;
using Deepin.Application.Queries;
using MediatR;

namespace Deepin.Application.Commands.Messages;

public class SearchMessageCommandHandler(IMessageQueries messageQueries) : IRequestHandler<SearchMessageCommand, IPagedResult<MessageDto>>
{
    public async Task<IPagedResult<MessageDto>> Handle(SearchMessageCommand command, CancellationToken cancellationToken)
    {
        var messages = await messageQueries.GetPagedAsync(command.Request, cancellationToken);

        return messages;
    }
}
