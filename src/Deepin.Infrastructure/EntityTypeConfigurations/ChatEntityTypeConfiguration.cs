using Deepin.Domain.ChatAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Deepin.Infrastructure.EntityTypeConfigurations;

public class ChatBaseEntityTypeConfiguration : IEntityTypeConfiguration<ChatBase>
{
    public void Configure(EntityTypeBuilder<ChatBase> builder)
    {
        builder.ToTable("chats");

        builder.HasDiscriminator<ChatType>("type")
            .HasValue<GroupChat>(ChatType.Group)
            .HasValue<DirectChat>(ChatType.Direct);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid").HasValueGenerator(typeof(SequentialGuidValueGenerator));

        builder.Property(x => x.CreatedBy).HasColumnName("created_by").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");
        builder.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        builder.HasMany(x => x.Members).WithOne().HasForeignKey("chat_id").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Settings).WithOne().HasForeignKey("chat_id").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.ReadStatuses).WithOne().HasForeignKey("chat_id").OnDelete(DeleteBehavior.Cascade);
    }
}
public class GroupChatEntityTypeConfiguration : IEntityTypeConfiguration<GroupChat>
{
    public void Configure(EntityTypeBuilder<GroupChat> builder)
    {
        builder.Property(x => x.Name).HasColumnName("name").IsRequired(false);
        builder.Property(x => x.UserName).HasColumnName("user_name").IsRequired(false);
        builder.Property(x => x.Description).HasColumnName("description").IsRequired(false);
        builder.Property(x => x.AvatarFileId).HasColumnName("avatar_file_id").HasColumnType("uuid").IsRequired(false);
        builder.Property(x => x.IsPublic).HasColumnName("is_public");
    }
}
public class DirectChatEntityTypeConfiguration : IEntityTypeConfiguration<DirectChat>
{
    public void Configure(EntityTypeBuilder<DirectChat> builder)
    {
    }
}