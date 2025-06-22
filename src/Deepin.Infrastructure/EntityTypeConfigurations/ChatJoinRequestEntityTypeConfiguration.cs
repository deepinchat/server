using Deepin.Domain.ChatAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Deepin.Infrastructure.EntityTypeConfigurations;

public class ChatJoinRequestEntityTypeConfiguration : IEntityTypeConfiguration<ChatJoinRequest>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ChatJoinRequest> builder)
    {
        builder.ToTable("chat_join_requests");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid").HasValueGenerator(typeof(SequentialGuidValueGenerator));

        builder.Property(x => x.ChatId).HasColumnName("chat_id").IsRequired();
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.Message).HasColumnName("message").IsRequired(false);
        builder.Property(x => x.Status).HasColumnName("status").IsRequired();
        builder.Property(x => x.ReviewedBy).HasColumnName("reviewed_by").HasColumnType("uuid").IsRequired(false);
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.ExpiresAt).HasColumnName("expires_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.ReviewedAt).HasColumnName("reviewed_at").HasColumnType("timestamp with time zone").IsRequired(false);

        // Indexes
        builder.HasIndex(x => new { x.ChatId, x.UserId }).IsUnique();
    }
}
