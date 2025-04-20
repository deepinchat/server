using Deepin.Domain.Emails;
using Deepin.Infrastructure.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Infrastructure.Data;

public class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : DbContext(options)
{
    public const string DEFAULT_SCHEMA = "notification";
    public required DbSet<Email> Emails { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
        modelBuilder.ApplyConfiguration(new EmailEntityTypeConfiguration());
    }
}
