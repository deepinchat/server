using Deepin.Domain.FileAggregate;
using Deepin.Infrastructure.EntityTypeConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Deepin.Infrastructure.Data;

public class StorageDbContext : DbContextBase<StorageDbContext>
{
    public const string DEFAULT_SCHEMA = "storage";
    public StorageDbContext(DbContextOptions<StorageDbContext> options, IMediator? mediator = null) : base(options, mediator)
    {
    }
    public DbSet<FileObject> FileObjects { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
        modelBuilder.ApplyConfiguration(new FileObjectEntityTypeConfiguration());
    }
}

//Create a design-time DbContext factory for migrations
public class StorageDbContextFactory : IDesignTimeDbContextFactory<StorageDbContext>
{
    public StorageDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StorageDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost");
        return new StorageDbContext(optionsBuilder.Options);
    }
}