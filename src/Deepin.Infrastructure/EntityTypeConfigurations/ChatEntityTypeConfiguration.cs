using Deepin.Domain.ChatAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Deepin.Infrastructure.EntityTypeConfigurations;

public class ChatEntityTypeConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("chats");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid").HasValueGenerator(typeof(SequentialGuidValueGenerator));

        builder.Property(x => x.CreatedBy).HasColumnName("created_by").IsRequired();
        builder.Property(x => x.Type).HasColumnName("type").HasConversion<string>().IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");

        builder.OwnsOne(c => c.GroupInfo, s =>
        {
            s.Property(x => x.Name).HasColumnName("name").IsRequired(false);
            s.Property(x => x.UserName).HasColumnName("user_name").IsRequired(false);
            s.Property(x => x.Description).HasColumnName("description").IsRequired(false);
            s.Property(x => x.AvatarFileId).HasColumnName("avatar_file_id").IsRequired(false);
            s.Property(x => x.IsPublic).HasColumnName("is_public");
        });

        builder.HasMany(x => x.Members).WithOne().HasForeignKey("chat_id");
        builder.HasMany(x => x.ReadStatuses).WithOne().HasForeignKey(x => x.ChatId);
    }
}
