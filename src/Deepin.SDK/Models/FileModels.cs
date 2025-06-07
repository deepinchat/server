namespace Deepin.SDK.Models;

/// <summary>
/// Represents a file in the system
/// </summary>
public class FileInfoModel
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Path { get; set; } = string.Empty;
    public int UploadedById { get; set; }
    public DateTime UploadedAt { get; set; }
    public User? UploadedBy { get; set; }
}

/// <summary>
/// Represents a file attachment
/// </summary>
public class FileAttachment
{
    public int Id { get; set; }
    public int FileId { get; set; }
    public int MessageId { get; set; }
    public FileInfoModel? File { get; set; }
}

/// <summary>
/// Response model for file upload
/// </summary>
public class FileUploadResponse
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
}

/// <summary>
/// Request model for file upload
/// </summary>
public class FileUploadRequest
{
    public Stream FileStream { get; set; } = Stream.Null;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}
