using Deepin.Application.DTOs.Files;

namespace Deepin.Application.Queries.Files;

public interface IFileQueries
{
    Task<FileDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FileDto?> GetByHashAsync(string hash, CancellationToken cancellationToken = default);
}
