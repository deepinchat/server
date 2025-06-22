using Deepin.Domain;
using Deepin.Domain.ChatAggregate;
using Deepin.Infrastructure.Data;

namespace Deepin.Infrastructure.Repositories;


public class DirectChatRepository(ChatDbContext db) : IDirectChatRepository
{
    private readonly ChatDbContext _db = db;
    public IUnitOfWork UnitOfWork => _db;

    public async Task AddAsync(DirectChat entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        await _db.DirectChats.AddAsync(entity);
    }

    public Task DeleteAsync(DirectChat entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        _db.DirectChats.Remove(entity);
        return Task.CompletedTask;
    }


    public async Task<DirectChat?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        return await _db.DirectChats.FindAsync(id, cancellationToken);
    }

    public Task UpdateAsync(DirectChat entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        _db.DirectChats.Update(entity);
        return Task.CompletedTask;
    }
}

