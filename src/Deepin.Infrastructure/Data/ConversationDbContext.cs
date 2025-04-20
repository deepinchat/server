using Deepin.Domain.ChatAggregate;
using Deepin.Infrastructure.EntityTypeConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Deepin.Infrastructure.Data;

public class ConversationDbContext : DbContextBase<ConversationDbContext>
{
    public const string DEFAULT_SCHEMA = "conversation";
    public ConversationDbContext(DbContextOptions<ConversationDbContext> options, IMediator? mediator = null) : base(options, mediator)
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
public class ChatDbContextFactory : IDesignTimeDbContextFactory<ConversationDbContext>
{
    public ConversationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ConversationDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost");
        return new ConversationDbContext(optionsBuilder.Options);
    }
}