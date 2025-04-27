namespace Deepin.Domain.FileAggregate;

public class FileObject : Entity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public string StorageKey { get; private set; }
    public string ContentType { get; private set; }
    public long Length { get; private set; }
    public string? ContainerName { get; private set; }
    public string Hash { get; private set; }
    public string Checksum { get; private set; }
    public string Format { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public StorageProvider Provider { get; private set; }

    private FileObject()
    {
        Name = string.Empty;
        StorageKey = string.Empty;
        ContentType = string.Empty;
        Hash = string.Empty;
        Checksum = string.Empty;
        Format = string.Empty;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public FileObject(
        Guid id,
        Guid uploaderUserId,
        string name,
        string storageKey,
        string contentType,
        long length,
        StorageProvider provider,
        string? containerName,
        string hash,
        string checksum,
        string format
        ) : this()
    {
        if (uploaderUserId == Guid.Empty)
            throw new ArgumentException("Uploader user ID cannot be empty.", nameof(uploaderUserId));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(storageKey))
            throw new ArgumentException("Storage key cannot be empty.", nameof(storageKey));
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("Content type cannot be empty.", nameof(contentType));
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length), "File length cannot be negative.");

        Id = id;
        CreatedBy = uploaderUserId;
        Name = name;
        StorageKey = storageKey;
        ContentType = contentType;
        Length = length;
        Provider = provider;
        ContainerName = containerName;
        Hash = hash ?? string.Empty;
        Checksum = checksum ?? string.Empty;
        Format = format ?? string.Empty;
    }
}
