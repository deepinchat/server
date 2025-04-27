using AutoMapper;
using Deepin.Application.DTOs.Chats;
using Deepin.Application.Interfaces;
using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public class CreateDirectChatCommandHandler(IMapper mapper, IChatRepository chatRepository, IUserContext userContext) : IRequestHandler<CreateDirectChatCommand, ChatDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly IUserContext _userContext = userContext;
    public async Task<ChatDto> Handle(CreateDirectChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new Chat(
            type: ChatType.Direct,
            createdBy: _userContext.UserId,
            groupInfo: null);
        request.UserIds.ToList().ForEach(userId => chat.AddMember(new ChatMember(userId, ChatMemberRole.Owner)));

        await _chatRepository.AddAsync(chat);
        await _chatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return _mapper.Map<ChatDto>(chat);
    }
}