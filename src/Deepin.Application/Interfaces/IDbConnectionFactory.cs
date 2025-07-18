using System.Data;

namespace Deepin.Application.Interfaces;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateChatDbConnectionAsync(CancellationToken cancellationToken = default);
    Task<IDbConnection> CreateMessageDbConnectionAsync(CancellationToken cancellationToken = default);
    Task<IDbConnection> CreateStorageDbConnectionAsync(CancellationToken cancellationToken = default);
    Task<IDbConnection> CreateIdentityDbConnectionAsync(CancellationToken cancellationToken = default);
    Task<IDbConnection> CreateContactDbConnectionAsync(CancellationToken cancellationToken = default);
}
