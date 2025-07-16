using Deepin.Domain;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity, TContext>(TContext db)
    : IRepository<TEntity>
    where TEntity : Entity
    where TContext : DbContext, IUnitOfWork
{
    public IUnitOfWork UnitOfWork => DbContext;

    protected TContext DbContext => db;

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await DbContext.Set<TEntity>().AddAsync(entity);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        DbContext.Set<TEntity>().Remove(entity);
        await Task.CompletedTask; // Simulate async operation
    }

    public async Task<TEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }

        return await DbContext.Set<TEntity>().FindAsync(id, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        DbContext.Set<TEntity>().Update(entity);
        await Task.CompletedTask; // Simulate async operation
    }
}