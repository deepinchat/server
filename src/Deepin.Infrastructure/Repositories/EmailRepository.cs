using Deepin.Domain;
using Deepin.Domain.Emails;
using Deepin.Infrastructure.Data;

namespace Deepin.Infrastructure.Repositories;

public class EmailRepository(NotificationDbContext db) : IEmailRepository
{
    public IUnitOfWork UnitOfWork => db;

    public async Task AddAsync(Email entity, CancellationToken cancellationToken = default)
    {
        await db.Emails.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(Email entity, CancellationToken cancellationToken = default)
    {
        db.Emails.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<Email?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.Emails
            .FindAsync([id], cancellationToken);
    }

    public async Task UpdateAsync(Email entity, CancellationToken cancellationToken = default)
    {
        db.Emails.Update(entity);
        await Task.CompletedTask;
    }
}
