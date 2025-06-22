using Deepin.Domain;
using Deepin.Domain.ChatAggregate;
using Deepin.Infrastructure.Data;

namespace Deepin.Infrastructure.Repositories;

public class GroupChatRepository(ChatDbContext db) : IGroupChatRepository
{
    private readonly ChatDbContext _db = db;
    public IUnitOfWork UnitOfWork => _db;
    public async Task AddAsync(GroupChat entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        await _db.GroupChats.AddAsync(entity);
    }

    public Task DeleteAsync(GroupChat entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        _db.GroupChats.Remove(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(GroupChat entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        _db.GroupChats.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<GroupChat?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        return await _db.GroupChats.FindAsync(id, cancellationToken);
    }
}

