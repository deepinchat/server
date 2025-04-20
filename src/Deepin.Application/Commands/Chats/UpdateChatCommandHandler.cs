using AutoMapper;
using Deepin.Application.Models.Chats;
using Deepin.Domain.ChatAggregate;
using Deepin.Domain.Exceptions;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public class UpdateChatCommandHandler(IMapper mapper, IChatRepository chatRepository) : IRequestHandler<UpdateChatCommand, ChatDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IChatRepository _chatRepository = chatRepository;
    public async Task<ChatDto> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = await _chatRepository.GetByIdAsync(request.Id, cancellationToken);
        if (chat is null) throw new DomainException($"Chat not found with id {request.Id}");
        if (chat.Type == ChatType.Direct || chat.GroupInfo is null) throw new DomainException("Chat is not a group chat");
        chat.UpdateGroupInfo(new GroupInfo(
            name: request.Name,
            userName: request.UserName,
            description: request.Description,
            avatarFileId: request.AvatarFileId,
            isPublic: request.IsPublic
        ));
        await _chatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return _mapper.Map<ChatDto>(chat);
    }
}
