using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Chats;

namespace Deepin.Application.Queries.Chats;

public interface IChatQueries
{
    Task<IEnumerable<ChatDto>> GetChats(Guid userId, CancellationToken cancellationToken = default);
    Task<ChatDto?> GetChatById(Guid id, CancellationToken cancellationToken = default);
    Task<ChatMemberDto?> GetChatMember(Guid chatId, Guid userId, CancellationToken cancellationToken = default);
    Task<IPagedResult<ChatMemberDto>> GetChatMembers(Guid chatId, int offset, int limit, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChatReadStatusDto>> GetChatReadStatusesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ChatReadStatusDto?> GetChatReadStatusAsync(Guid chatId, Guid userId, CancellationToken cancellationToken = default);
}
