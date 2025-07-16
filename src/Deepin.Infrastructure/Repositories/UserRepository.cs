using Deepin.Domain;
using Deepin.Domain.Identity;
using Deepin.Infrastructure.Data;

namespace Deepin.Infrastructure.Repositories;

public class UserRepository(IdentityContext db) : IUserRepository
{
    public IUnitOfWork UnitOfWork => db;

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await db.Users.AddAsync(user);
    }

    public Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        db.Users.Remove(user);
        return Task.CompletedTask;
    }

    public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty) throw new ArgumentException("User ID cannot be empty.", nameof(id));

        var user = await db.Users.FindAsync([id], cancellationToken);
        if (user == null)
        {
            return null;
        }
        // Ensure claims are loaded
        await db.Entry(user).Collection(u => u.Claims).LoadAsync(cancellationToken);
        return user;
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        db.Users.Update(user);
        return Task.CompletedTask;
    }
}
