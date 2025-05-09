using Deepin.Domain.MessageAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Deepin.Infrastructure.EntityTypeConfigurations;

public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.ChatId, x.CreatedAt });
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid");
        builder.Property(x => x.ChatId).HasColumnName("chat_id").HasColumnType("uuid").IsRequired();
        builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("uuid").IsRequired();
        builder.Property(x => x.Type).HasColumnName("type").HasColumnType("text").HasConversion<string>().IsRequired();
        builder.Property(x => x.Text).HasColumnName("text").HasColumnType("text").IsRequired(false);
        builder.Property(x => x.Mentions).HasColumnName("mentions").HasColumnType("jsonb").IsRequired(false);
        builder.Property(x => x.Metadata).HasColumnName("metadata").HasColumnType("jsonb").IsRequired(false);
        builder.Property(x => x.ParentId).HasColumnName("parent_id").HasColumnType("uuid").IsRequired(false);
        builder.Property(x => x.ReplyToId).HasColumnName("reply_id").HasColumnType("uuid").IsRequired(false);
        builder.Property(x => x.ReplyToId).HasColumnName("reply_id").HasColumnType("uuid").IsRequired(false);
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");
        builder.Property(x => x.IsEdited).HasColumnName("is_edited");
        builder.Property(x => x.IsPinned).HasColumnName("is_pinned");
        builder.Property(x => x.IsRead).HasColumnName("is_read");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
        builder.Property(x => x.ModifiedAt).HasColumnName("modified_at").HasColumnType("timestamp with time zone").ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        builder.HasMany(x => x.Attachments).WithOne().HasForeignKey("chat_id");
    }
}
