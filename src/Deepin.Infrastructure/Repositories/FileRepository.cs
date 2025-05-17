using Deepin.Domain;
using Deepin.Domain.FileAggregate;
using Deepin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class FileRepository(StorageDbContext db) : IFileRepository
{
    public IUnitOfWork UnitOfWork => db;

    public async Task AddAsync(FileObject entity, CancellationToken cancellationToken = default)
    {
        await db.FileObjects.AddAsync(entity, cancellationToken);
    }

    public Task DeleteAsync(FileObject entity, CancellationToken cancellationToken = default)
    {
        db.FileObjects.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<FileObject?> FindByHashAsync(string hash, CancellationToken cancellationToken = default)
    {
        return db.FileObjects.FirstOrDefaultAsync(x => x.Hash == hash, cancellationToken);
    }

    public async Task<FileObject?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.FileObjects.FindAsync([id], cancellationToken);
    }

    public Task UpdateAsync(FileObject entity, CancellationToken cancellationToken = default)
    {
        db.FileObjects.Update(entity);
        return Task.CompletedTask;
    }
}