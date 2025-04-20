using Deepin.Domain;
using Deepin.Domain.ChatAggregate;
using Deepin.Infrastructure.Data;

namespace Deepin.Infrastructure.Repositories;
public class ChatRepository(ConversationDbContext db) : IChatRepository
{
    private readonly ConversationDbContext _db = db;
    public IUnitOfWork UnitOfWork =>_db;

    public Chat Add(Chat chat)
    {
        return _db.Chats.Add(chat).Entity;
    }

    public async Task<Chat?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.Chats.FindAsync([id], cancellationToken);
    }

    public Chat Update(Chat chat)
    {
        return _db.Chats.Update(chat).Entity;
    }
}

