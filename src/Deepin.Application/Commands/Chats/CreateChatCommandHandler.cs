using AutoMapper;
using Deepin.Application.DTOs.Chats;
using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public class CreateChatCommandHandler(IMapper mapper, IChatRepository chatRepository) : IRequestHandler<CreateChatCommand, ChatDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IChatRepository _chatRepository = chatRepository;
    public async Task<ChatDto> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new Chat(
            type: request.Type,
            createdBy: request.OwnerId,
            groupInfo: new GroupInfo(
                name: request.Name,
                userName: request.UserName,
                description: request.Description,
                avatarFileId: request.AvatarFileId,
                isPublic: request.IsPublic
            ));
        await _chatRepository.AddAsync(chat);
        await _chatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return _mapper.Map<ChatDto>(chat);
    }
}
