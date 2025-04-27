using System.Data;
using Deepin.Application.Interfaces;
using Deepin.Infrastructure.Configurations;

namespace Deepin.Infrastructure.Data;

public class NpgsqlDbConnectionFactory(AppOptions appOptions) : IDbConnectionFactory
{
    public async Task<IDbConnection> CreateChatDbConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = appOptions.ConnectionStrings.Chat ?? appOptions.ConnectionStrings.Default;
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException($"Chat Connection string not found.");
        }

        return await CreateDbConnectionAsync(connectionString, ChatDbContext.DEFAULT_SCHEMA, cancellationToken);
    }
    public async Task<IDbConnection> CreateStorageDbConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = appOptions.ConnectionStrings.Storage ?? appOptions.ConnectionStrings.Default;
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException($"Chat Connection string not found.");
        }
        return await CreateDbConnectionAsync(connectionString, StorageDbContext.DEFAULT_SCHEMA, cancellationToken);
    }
    public async Task<IDbConnection> CreateDbConnectionAsync(string connectionString, string schema, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException($"Connection string not found.");
        }
        var connection = new Npgsql.NpgsqlConnection(connectionString);
        try
        {
            await connection.OpenAsync(cancellationToken);
            await using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SET search_path TO {schema}";
                command.ExecuteNonQuery();
            }
            return connection; // Return as IDbConnection
        }
        catch
        {
            // If OpenAsync fails, ensure the connection is disposed.
            await connection.DisposeAsync();
            throw;
        }
    }
}
