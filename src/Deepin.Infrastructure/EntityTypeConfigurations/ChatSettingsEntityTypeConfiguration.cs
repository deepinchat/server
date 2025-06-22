using Deepin.Domain.ChatAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Deepin.Infrastructure.EntityTypeConfigurations;

public class ChatSettingsEntityTypeConfiguration : IEntityTypeConfiguration<ChatSettings>
{
    public void Configure(EntityTypeBuilder<ChatSettings> builder)
    {
        builder.ToTable("chat_settings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid").HasValueGenerator(typeof(SequentialGuidValueGenerator));
        builder.Property<Guid>("chat_id").HasColumnType("uuid").IsRequired();

        builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("uuid").IsRequired();
        builder.Property(x => x.IsPinned).HasColumnName("is_pinned").IsRequired().HasDefaultValue(false);
        builder.Property(x => x.IsMuted).HasColumnName("is_muted").IsRequired().HasDefaultValue(false);
        builder.Property(x => x.NotificationLevel)
            .HasColumnName("notification_level")
            .HasConversion<string>()
            .IsRequired()
            .HasDefaultValue(ChatNotificationLevel.All);

        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone").IsRequired();
    }
}
