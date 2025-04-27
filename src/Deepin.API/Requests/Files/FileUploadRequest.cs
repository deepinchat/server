namespace Deepin.API.Requests.Files;

public class FileUploadRequest
{
    public IFormFile File { get; set; } = null!;
    public string? ContainerName { get; set; }
    public string? StorageKey { get; set; }
}
