using Deepin.Domain.MessageAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Deepin.Infrastructure.EntityTypeConfigurations;

public class MessageAttachmentEntityTypeConfiguration : IEntityTypeConfiguration<MessageAttachment>
{
    public void Configure(EntityTypeBuilder<MessageAttachment> builder)
    {
        builder.ToTable("message_attachments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid");
        builder.Property(x => x.Type).HasColumnName("type").HasColumnType("text").HasConversion<string>().IsRequired();
        builder.Property(x => x.FileId).HasColumnName("file_id").HasColumnType("uuid").IsRequired();
        builder.Property(x => x.FileName).HasColumnName("file_name").HasColumnType("text").IsRequired();
        builder.Property(x => x.FileSize).HasColumnName("file_size");
        builder.Property(x => x.ContentType).HasColumnName("content_type").HasColumnType("text").IsRequired();
        builder.Property(x => x.Metadata).HasColumnName("metadata").HasColumnType("jsonb").IsRequired(false);
        builder.Property(x => x.ThumbnailFileId).HasColumnName("thumbnail_file_id").HasColumnType("uuid").IsRequired(false);
        builder.Property(x => x.Order).HasColumnName("order").IsRequired();

    }
}
