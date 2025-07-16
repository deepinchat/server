using Deepin.Domain.ChatAggregate;
using Deepin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Infrastructure.Repositories;

public class ChatReadStatusRepository : RepositoryBase<ChatReadStatus, ChatDbContext>, IChatReadStatusRepository
{
    public ChatReadStatusRepository(ChatDbContext db) : base(db)
    {
    }

    public async Task<ChatReadStatus?> GetByChatIdAsync(Guid chatId, Guid userId, CancellationToken cancellationToken = default)
    {
        if (chatId == Guid.Empty)
        {
            throw new ArgumentException("Chat ID cannot be empty.", nameof(chatId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        return await DbContext.ChatReadStatuses
            .FirstOrDefaultAsync(crs => crs.ChatId == chatId && crs.UserId == userId, cancellationToken);
    }
}
