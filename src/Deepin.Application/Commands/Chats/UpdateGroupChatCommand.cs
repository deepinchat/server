using AutoMapper;
using Deepin.Application.DTOs.Chats;
using Deepin.Domain.ChatAggregate;
using Deepin.Domain.Exceptions;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record UpdateGroupChatCommand(
    Guid Id,
    string Name,
    string? UserName,
    string? Description,
    Guid? AvatarFileId,
    bool IsPublic) : IRequest<GroupChatDto>;

public class UpdateGroupChatCommandHandler(IMapper mapper, IGroupChatRepository groupChatRepository) : IRequestHandler<UpdateGroupChatCommand, GroupChatDto>
{
    public async Task<GroupChatDto> Handle(UpdateGroupChatCommand request, CancellationToken cancellationToken)
    {
        var groupChat = await groupChatRepository.FindByIdAsync(request.Id, cancellationToken);
        if (groupChat is null)
        {
            throw new EntityNotFoundException(nameof(GroupChat), request.Id);
        }

        groupChat.Update(
            name: request.Name,
            userName: request.UserName,
            description: request.Description,
            avatarFileId: request.AvatarFileId,
            isPublic: request.IsPublic
        );

        await groupChatRepository.UpdateAsync(groupChat, cancellationToken);
        await groupChatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return mapper.Map<GroupChatDto>(groupChat);
    }
}