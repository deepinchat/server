using Deepin.Application.Models.Chats;
using Deepin.Infrastructure.Pagination;

namespace Deepin.Application.Queries.Chats;

public interface IChatQueries
{
    Task<IEnumerable<ChatDto>> GetChats(string userId);
    Task<ChatDto?> GetChatById(Guid chatId);
    Task<ChatMemberDto?> GetChatMember(Guid chatId, string userId);
    Task<IPagination<ChatMemberDto>> GetChatMembers(Guid chatId, int offset, int limit);
    Task<IEnumerable<ChatReadStatusDto>> GetChatReadStatusesAsync(string userId);
    Task<ChatReadStatusDto?> GetChatReadStatusAsync(Guid chatId, string userId);
}
