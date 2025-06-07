using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Infrastructure.Data;

public class ConfigurationContext : ConfigurationDbContext<ConfigurationContext>
{
    public ConfigurationContext(DbContextOptions<ConfigurationContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("idsv");
    }
}