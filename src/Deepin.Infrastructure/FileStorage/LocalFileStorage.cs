using Deepin.Application.DTOs.Files;
using Deepin.Application.Interfaces;
using Deepin.Domain.FileAggregate;
using Deepin.Infrastructure.Configurations;

namespace Deepin.Infrastructure.FileStorage;

public class LocalFileStorageOptions
{
    public required string RootPath { get; set; }
}
public class LocalFileStorage(StorageOptions options) : IFileStorage
{
    private readonly string _rootPath = options.FileSystem?.RootPath
        ?? throw new ArgumentNullException(nameof(options.FileSystem), "Local file storage root path is not configured.");

    public StorageProvider Provider => StorageProvider.Local;

    private string GetFullPath(FileDto file)
    {
        var fullPath = Path.Combine(_rootPath, file.StorageKey);
        return fullPath;
    }
    public async Task CreateAsync(FileDto file, Stream stream, CancellationToken cancellationToken = default)
    {
        var fullPath = GetFullPath(file);
        var dir = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        var fileInfo = new FileInfo(fullPath);
        using var fs = fileInfo.Create();
        await stream.CopyToAsync(fs);
        await fs.FlushAsync();
    }

    public async Task DeleteAsync(FileDto file, CancellationToken cancellationToken = default)
    {
        var fullPath = GetFullPath(file);
        var fileInfo = new FileInfo(fullPath);
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }
        await Task.CompletedTask;
    }

    public async Task<Stream?> GetStreamAsync(FileDto file, CancellationToken cancellationToken = default)
    {
        var fullPath = GetFullPath(file);
        var fileInfo = new FileInfo(fullPath);
        if (!fileInfo.Exists)
            return null;
        return await Task.FromResult(fileInfo.OpenRead());
    }

    public Task<bool> ExistsAsync(FileDto file, CancellationToken cancellationToken = default)
    {
        var fullPath = GetFullPath(file);
        var fileInfo = new FileInfo(fullPath);
        return Task.FromResult(fileInfo.Exists);
    }

    public Task<string> BuildStorageKeyAsync(string hash, string? containerName = null, string? storageKey = null, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Path.Combine(containerName ?? DateTimeOffset.UtcNow.ToString("yyyy/MM"), hash));
    }
}
