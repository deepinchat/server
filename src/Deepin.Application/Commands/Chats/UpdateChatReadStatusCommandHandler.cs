using Deepin.Domain;
using Deepin.Domain.ChatAggregate;
using Deepin.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Application.Commands.Chats;

public class UpdateChatReadStatusCommandHandler(ConversationDbContext dbContext, IUserContext userContext) : IRequestHandler<UpdateChatReadStatusCommand, bool>
{
    private readonly ConversationDbContext _dbContext = dbContext;
    private readonly IUserContext _userContext = userContext;
    public async Task<bool> Handle(UpdateChatReadStatusCommand request, CancellationToken cancellationToken)
    {
        var chatReadStatus = await _dbContext.ChatReadStatuses
            .FirstOrDefaultAsync(x => x.ChatId == request.ChatId && x.UserId == _userContext.UserId, cancellationToken);
        if (chatReadStatus is null)
        {
            chatReadStatus = new ChatReadStatus(request.ChatId, _userContext.UserId, request.MessageId);
            _dbContext.ChatReadStatuses.Add(chatReadStatus);
        }
        else
        {
            chatReadStatus.ReadMessage(request.MessageId);
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
