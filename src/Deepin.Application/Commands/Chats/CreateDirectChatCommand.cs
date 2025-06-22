using AutoMapper;
using Deepin.Application.DTOs.Chats;
using Deepin.Application.Interfaces;
using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record CreateDirectChatCommand(Guid OtherUserId) : IRequest<ChatDto>;
public class CreateDirectChatCommandHandler(
    IMapper mapper,
    IDirectChatRepository repository,
    IUserContext userContext) : IRequestHandler<CreateDirectChatCommand, ChatDto>
{
    public async Task<ChatDto> Handle(CreateDirectChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new DirectChat(userContext.UserId, request.OtherUserId);

        await repository.AddAsync(chat);
        await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return mapper.Map<ChatDto>(chat);
    }
}