namespace Deepin.Domain.FileAggregate;

public interface IFileRepository : IRepository<FileObject>
{
    Task<FileObject?> FindByHashAsync(string hash, CancellationToken cancellationToken = default);
}
