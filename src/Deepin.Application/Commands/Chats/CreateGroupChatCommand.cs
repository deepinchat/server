using AutoMapper;
using Deepin.Application.DTOs.Chats;
using Deepin.Application.Interfaces;
using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record CreateChatCommand(
    string Name,
    string UserName,
    string Description,
    Guid? AvatarFileId,
    bool IsPublic) : IRequest<ChatDto>;
public class CreateGroupChatCommandHandler(
    IMapper mapper,
    IGroupChatRepository repository,
    IUserContext userContext) : IRequestHandler<CreateChatCommand, ChatDto>
{
    public async Task<ChatDto> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new GroupChat(
            userContext.UserId,
            request.Name,
            request.UserName,
            request.Description,
            request.AvatarFileId,
            request.IsPublic);
        await repository.AddAsync(chat);
        await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return mapper.Map<ChatDto>(chat);
    }
}
