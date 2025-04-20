using Deepin.Domain.ChatAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Deepin.Infrastructure.EntityTypeConfigurations;

public class ChatMemberEntityTypeConfiguration : IEntityTypeConfiguration<ChatMember>
{
    public void Configure(EntityTypeBuilder<ChatMember> builder)
    {
        builder.ToTable("chat_members");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid").HasValueGenerator(typeof(SequentialGuidValueGenerator));

        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.JoinedAt).HasColumnName("joined_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.Role).HasColumnName("role").HasConversion<string>().IsRequired();
        builder.Property(x => x.DisplayName).HasColumnName("display_name").IsRequired(false);

        builder.Property<Guid>("chat_id").HasColumnType("uuid").IsRequired();
    }
}
