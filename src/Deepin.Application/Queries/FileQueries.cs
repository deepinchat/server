using Dapper;
using Deepin.Application.DTOs.Files;
using Deepin.Application.Interfaces;
using Deepin.Chatting.Application.Constants;
using Deepin.Domain.FileAggregate;

namespace Deepin.Application.Queries;
public interface IFileQueries
{
    Task<FileDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FileDto?> GetByHashAsync(string hash, CancellationToken cancellationToken = default);
}

public class FileQueries(IDbConnectionFactory dbConnectionFactory, ICacheManager cacheManager) : IFileQueries
{
    public async Task<FileDto?> GetByHashAsync(string hash, CancellationToken cancellationToken = default)
    {
        using (var connection = await dbConnectionFactory.CreateStorageDbConnectionAsync(cancellationToken))
        {
            var sql = @"SELECT * FROM file_objects WHERE hash = @hash";
            var command = new CommandDefinition(sql, new { hash }, cancellationToken: cancellationToken);
            var result = await connection.QueryFirstOrDefaultAsync<dynamic>(command);
            return result is null ? null : MapFileDto(result);
        }
    }

    public async Task<FileDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeys.GetFileByIdCacheKey(id);
        return await cacheManager.GetOrSetAsync<FileDto?>(cacheKey, async () =>
        {
            using (var connection = await dbConnectionFactory.CreateStorageDbConnectionAsync(cancellationToken))
            {
                var sql = @"SELECT * FROM file_objects WHERE id = @id";
                var command = new CommandDefinition(sql, new { id }, cancellationToken: cancellationToken);
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(command);
                return result is null ? null : MapFileDto(result);
            }
        });
    }
    private FileDto MapFileDto(dynamic row)
    {
        return new FileDto
        {
            Id = row.id,
            Name = row.name,
            CreatedAt = row.created_at,
            UpdatedAt = row.updated_at,
            Checksum = row.checksum,
            ContentType = row.content_type,
            ContainerName = row.container_name,
            CreatedBy = row.created_by,
            Format = row.format,
            Hash = row.hash,
            Length = row.length,
            StorageKey = row.storage_key,
            Provider = Enum.Parse<StorageProvider>(row.provider.ToString(), true),
        };
    }
}