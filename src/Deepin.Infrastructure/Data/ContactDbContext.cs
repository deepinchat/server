using Deepin.Domain.ContactAggregate;
using Deepin.Infrastructure.EntityTypeConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Deepin.Infrastructure.Data;
public class ContactDbContext : DbContextBase<ContactDbContext>
{
    public const string DEFAULT_SCHEMA = "contacts";
    public ContactDbContext(DbContextOptions<ContactDbContext> options, IMediator? mediator = null) : base(options, mediator)
    {
    }
    public DbSet<Contact> Contacts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
        modelBuilder.ApplyConfiguration(new ContactEntityTypeConfiguration());
    }
}
public class ContactDbContextFactory : IDesignTimeDbContextFactory<ContactDbContext>
{
    public ContactDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ContactDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost");
        return new ContactDbContext(optionsBuilder.Options);
    }
}