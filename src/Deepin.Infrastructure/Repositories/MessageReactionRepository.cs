using Deepin.Domain;
using Deepin.Domain.MessageAggregate;
using Deepin.Infrastructure.Data;

namespace Deepin.Infrastructure.Repositories;

public class MessageReactionRepository(MessageDbContext db) : IMessageReactionRepository
{
    public IUnitOfWork UnitOfWork => db;

    public async Task AddAsync(MessageReaction entity, CancellationToken cancellationToken = default)
    {
        await db.MessageReactions.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(MessageReaction entity, CancellationToken cancellationToken = default)
    {
        db.MessageReactions.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<MessageReaction?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.MessageReactions.FindAsync([id], cancellationToken);
    }

    public async Task UpdateAsync(MessageReaction entity, CancellationToken cancellationToken = default)
    {
        db.MessageReactions.Update(entity);
        await Task.CompletedTask;
    }
}
