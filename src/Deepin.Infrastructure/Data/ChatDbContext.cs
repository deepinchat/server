using Deepin.Domain.ChatAggregate;
using Deepin.Infrastructure.EntityTypeConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Deepin.Infrastructure.Data;

public class ChatDbContext : DbContextBase<ChatDbContext>
{
    public const string DEFAULT_SCHEMA = "chats";
    public ChatDbContext(DbContextOptions<ChatDbContext> options, IMediator? mediator = null) : base(options, mediator)
    {
    }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatReadStatus> ChatReadStatuses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
        modelBuilder.ApplyConfiguration(new ChatEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ChatMemberEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ChatReadStatusEntityTypeConfiguration());
    }
}

//Create a design-time DbContext factory for migrations
public class ChatDbContextFactory : IDesignTimeDbContextFactory<ChatDbContext>
{
    public ChatDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChatDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost");
        return new ChatDbContext(optionsBuilder.Options);
    }
}