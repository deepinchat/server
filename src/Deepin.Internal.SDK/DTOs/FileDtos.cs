using Deepin.Internal.SDK.Enums;

namespace Deepin.Internal.SDK.Models;

/// <summary>
/// Represents a file in the system
/// </summary>
public class FileInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StorageKey { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Length { get; set; }
    public string? ContainerName { get; set; }
    public string Hash { get; set; } = string.Empty;
    public string Checksum { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public StorageProvider Provider { get; set; }
}
/// <summary>
/// Request model for file upload
/// </summary>
public class UploadFileRequest
{
    public Stream FileStream { get; set; } = Stream.Null;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}
