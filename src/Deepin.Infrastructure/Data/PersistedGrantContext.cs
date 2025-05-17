using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Infrastructure.Data;

public class PersistedGrantContext : PersistedGrantDbContext<PersistedGrantContext>
{
    public PersistedGrantContext(DbContextOptions<PersistedGrantContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("idsv");
    }
}