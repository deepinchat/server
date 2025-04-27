using Deepin.Application.DTOs.Files;
using Deepin.Application.Interfaces;
using Deepin.Domain.FileAggregate;

namespace Deepin.Infrastructure.FileStorage;

public class S3FileStorage : IFileStorage
{
    public StorageProvider Provider => StorageProvider.AmazonS3;

    public Task<string> BuildStorageKeyAsync(Guid id, string fileName, string? containerName = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(FileDto file, Stream stream, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(FileDto file, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(FileDto file, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetStreamAsync(FileDto file, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
