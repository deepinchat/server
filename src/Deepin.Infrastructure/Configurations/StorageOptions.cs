using Deepin.Domain.FileAggregate;
using Deepin.Infrastructure.FileStorage;

namespace Deepin.Infrastructure.Configurations;

public class StorageOptions
{
    public StorageProvider Provider { get; set; } = StorageProvider.Local;
    public LocalFileStorageOptions? FileSystem { get; set; }
    public S3FileStorageOptions? S3 { get; set; }
}
