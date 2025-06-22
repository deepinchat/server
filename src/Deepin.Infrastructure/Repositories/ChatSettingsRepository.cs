using Deepin.Domain.ChatAggregate;
using Deepin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Infrastructure.Repositories;

public class ChatSettingsRepository : RepositoryBase<ChatSettings, ChatDbContext>, IChatSettingsRepository
{
    public ChatSettingsRepository(ChatDbContext db) : base(db)
    {
    }

    public async Task<ChatSettings?> GetByChatIdAsync(Guid chatId, Guid userId, CancellationToken cancellationToken = default)
    {
        if (chatId == Guid.Empty)
        {
            throw new ArgumentException("Chat ID cannot be empty.", nameof(chatId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        return await DbContext.ChatSettings
            .FirstOrDefaultAsync(cs => cs.ChatId == chatId && cs.UserId == userId, cancellationToken);
    }
}
