using AutoMapper;
using Deepin.Application.Models.Chats;
using Deepin.Domain;
using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public class CreateChatCommandHandler(IMapper mapper, IChatRepository chatRepository, IUserContext userContext) : IRequestHandler<CreateChatCommand, ChatDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly IUserContext _userContext = userContext;
    public async Task<ChatDto> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new Chat(
            type: request.Type,
            createdBy: _userContext.UserId,
            groupInfo: new GroupInfo(
                name: request.Name,
                userName: request.UserName,
                description: request.Description,
                avatarFileId: request.AvatarFileId,
                isPublic: request.IsPublic
            ));
        _chatRepository.Add(chat);
        await _chatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return _mapper.Map<ChatDto>(chat);
    }
}
