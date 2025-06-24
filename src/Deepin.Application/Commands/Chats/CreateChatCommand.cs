using AutoMapper;
using Deepin.Application.DTOs.Chats;
using Deepin.Application.Interfaces;
using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record CreateGroupChatCommand(
    string Name,
    string? UserName,
    string? Description,
    Guid? AvatarFileId,
    bool IsPublic) : IRequest<GroupChatDto>;
public record CreateDirectChatCommand(Guid OtherUserId) : IRequest<DirectChatDto>;
public class CreateDirectChatCommandHandler(
    IMapper mapper,
    IDirectChatRepository directChatRepository,
    IGroupChatRepository groupChatRepository,
    IUserContext userContext) :
    IRequestHandler<CreateDirectChatCommand, DirectChatDto>,
    IRequestHandler<CreateGroupChatCommand, GroupChatDto>
{
    public async Task<DirectChatDto> Handle(CreateDirectChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new DirectChat(userContext.UserId, request.OtherUserId);

        await directChatRepository.AddAsync(chat);
        await directChatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return mapper.Map<DirectChatDto>(chat);
    }

    public async Task<GroupChatDto> Handle(CreateGroupChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new GroupChat(
            userContext.UserId,
            request.Name,
            request.UserName,
            request.Description,
            request.AvatarFileId,
            request.IsPublic);
        await groupChatRepository.AddAsync(chat);
        await groupChatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return mapper.Map<GroupChatDto>(chat);
    }
}