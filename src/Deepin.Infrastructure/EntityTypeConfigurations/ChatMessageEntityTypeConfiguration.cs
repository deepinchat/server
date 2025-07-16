using Deepin.Domain.ChatAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Deepin.Infrastructure.EntityTypeConfigurations;

public class ChatMessageEntityTypeConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("chat_messages");
        builder.HasKey(x => x.Id);
        builder.HasIndex(m => new { m.ChatId, m.MessageId })
            .IsUnique();

        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid").HasValueGenerator(typeof(SequentialGuidValueGenerator));
        builder.Property(x => x.ChatId).HasColumnName("chat_id").IsRequired();
        builder.Property(x => x.MessageId).HasColumnName("message_id").HasColumnType("uuid").IsRequired();
        builder.Property(x => x.Type).HasColumnName("type").HasConversion<string>().IsRequired();
        builder.Property(x => x.SenderId).HasColumnName("sender_id").HasColumnType("uuid").IsRequired(false);
        builder.Property(x => x.SentAt).HasColumnName("sent_at").HasColumnType("timestamp with time zone").IsRequired();
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").HasColumnType("boolean").HasDefaultValue(false);
    }
}
