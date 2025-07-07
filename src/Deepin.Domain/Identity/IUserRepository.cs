namespace Deepin.Domain.Identity;

public interface IUserRepository
{
    IUnitOfWork UnitOfWork { get; }
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(User user, CancellationToken cancellationToken = default);
}
