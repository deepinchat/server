using AutoMapper;
using Deepin.Application.DTOs.Chats;
using Deepin.Application.Queries;
using Deepin.Domain.ChatAggregate;
using Deepin.Domain.Exceptions;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public class CreateDirectChatCommandHandler(IMapper mapper, IChatRepository chatRepository, IUserQueries userQueries) : IRequestHandler<CreateDirectChatCommand, ChatDto>
{
    public async Task<ChatDto> Handle(CreateDirectChatCommand request, CancellationToken cancellationToken)
    {
        var users = await userQueries.GetUsersAsync(request.Others, cancellationToken);

        if (users == null || !users.Any())
        {
            throw new EntityNotFoundException($"User {string.Join(", ", request.Others)} not found.");
        }
        var nonExistentUsers = request.Others.Where(userId => !users.Any(x => x.Id == userId)).ToList();
        if (nonExistentUsers.Any())
        {
            throw new EntityNotFoundException($"User {string.Join(", ", nonExistentUsers)} not found.");
        }
        var chat = new Chat(
            type: ChatType.Direct,
            createdBy: request.OwnerId,
            groupInfo: null);
        request.Others.ToList().ForEach(userId => chat.AddMember(new ChatMember(userId, ChatMemberRole.Owner)));

        await chatRepository.AddAsync(chat);
        await chatRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return mapper.Map<ChatDto>(chat);
    }
}