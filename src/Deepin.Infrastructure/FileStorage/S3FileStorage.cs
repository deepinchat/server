using Deepin.Application.DTOs.Files;
using Deepin.Application.Interfaces;
using Deepin.Domain.FileAggregate;
using Deepin.Infrastructure.Configurations;

namespace Deepin.Infrastructure.FileStorage;

public class S3FileStorageOptions
{
    public string? BucketName { get; set; }
    public string? Region { get; set; }
    public string? AccessKey { get; set; }
    public string? SecretKey { get; set; }
}
public class S3FileStorage(StorageOptions options) : IFileStorage
{
    public StorageProvider Provider => StorageProvider.AwsS3;
    private readonly S3FileStorageOptions _options = options.S3
        ?? throw new ArgumentNullException(nameof(options.S3), "S3 file storage options are not configured.");

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
