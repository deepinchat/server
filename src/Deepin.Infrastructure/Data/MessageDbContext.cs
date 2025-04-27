using Deepin.Domain.MessageAggregate;
using Deepin.Infrastructure.EntityTypeConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Deepin.Infrastructure.Data;

public class MessageDbContext : DbContextBase<MessageDbContext>
{
    public const string DEFAULT_SCHEMA = "messages";
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageReaction> MessageReactions { get; set; }
    public MessageDbContext(DbContextOptions<MessageDbContext> options, IMediator? mediator = null) : base(options, mediator)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
        modelBuilder.ApplyConfiguration(new MessageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MessageReactionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MessageAttachmentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MessageMentionEntityTypeConfiguration());
    }
}
public class MessageDbContextFactory : IDesignTimeDbContextFactory<MessageDbContext>
{
    public MessageDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MessageDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost");
        return new MessageDbContext(optionsBuilder.Options);
    }
}