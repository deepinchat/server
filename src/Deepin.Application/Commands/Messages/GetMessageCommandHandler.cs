using Deepin.Application.DTOs.Messages;
using Deepin.Application.Queries;
using Deepin.Domain.Exceptions;
using Deepin.Domain.MessageAggregate;
using MediatR;

namespace Deepin.Application.Commands.Messages;

public class GetMessageCommandHandler(IMessageQueries messageQueries) : IRequestHandler<GetMessageCommand, MessageDto?>
{
    public async Task<MessageDto?> Handle(GetMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await messageQueries.GetMessageAsync(request.Id, cancellationToken) ?? throw new EntityNotFoundException(
                nameof(Message),
                request.Id);
        return message;
    }
}
