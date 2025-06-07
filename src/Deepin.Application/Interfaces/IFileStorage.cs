using Deepin.Application.DTOs.Files;
using Deepin.Domain.FileAggregate;

namespace Deepin.Application.Interfaces;

public interface IFileStorage
{
    StorageProvider Provider { get; }
    Task CreateAsync(FileDto file, Stream stream, CancellationToken cancellationToken = default);
    Task DeleteAsync(FileDto file, CancellationToken cancellationToken = default);
    Task<Stream?> GetStreamAsync(FileDto file, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(FileDto file, CancellationToken cancellationToken = default);
    Task<string> BuildStorageKeyAsync(string hash, string? containerName = null, string? storageKey = null, CancellationToken cancellationToken = default);
}
