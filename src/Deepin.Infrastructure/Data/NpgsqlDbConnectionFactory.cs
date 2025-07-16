using System.Data;
using Deepin.Application.Interfaces;
using Deepin.Infrastructure.Configurations;

namespace Deepin.Infrastructure.Data;

public class NpgsqlDbConnectionFactory(ConnectionStringOptions connectionStrings) : IDbConnectionFactory
{
    public async Task<IDbConnection> CreateChatDbConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(connectionStrings.Chat))
        {
            throw new ArgumentException($"Chat Connection string not found.");
        }

        return await CreateDbConnectionAsync(connectionStrings.Chat, ChatDbContext.DEFAULT_SCHEMA, cancellationToken);
    }

    public async Task<IDbConnection> CreateMessageDbConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(connectionStrings.Chat))
        {
            throw new ArgumentException($"Message Connection string not found.");
        }

        return await CreateDbConnectionAsync(connectionStrings.Message, MessageDbContext.DEFAULT_SCHEMA, cancellationToken);
    }
    public async Task<IDbConnection> CreateStorageDbConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(connectionStrings.Storage))
        {
            throw new ArgumentException($"Chat Connection string not found.");
        }
        return await CreateDbConnectionAsync(connectionStrings.Storage, StorageDbContext.DEFAULT_SCHEMA, cancellationToken);
    }

    public async Task<IDbConnection> CreateIdentityDbConnectionAsync(CancellationToken cancellationToken = default)
    { 
        if (string.IsNullOrEmpty(connectionStrings.Identity))
        {
            throw new ArgumentException($"Identity Connection string not found.");
        }
        return await CreateDbConnectionAsync(connectionStrings.Identity, IdentityContext.DEFAULT_SCHEMA, cancellationToken);
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

    public async Task<IDbConnection> CreateContactDbConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(connectionStrings.Contact))
        {
            throw new ArgumentException($"Contact Connection string not found.");
        }
        return await CreateDbConnectionAsync(connectionStrings.Contact, ContactDbContext.DEFAULT_SCHEMA, cancellationToken);
    }
}
