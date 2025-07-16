namespace Deepin.Domain;

public interface IRepository<T> where T : Entity
{
    IUnitOfWork UnitOfWork { get; }
    Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
