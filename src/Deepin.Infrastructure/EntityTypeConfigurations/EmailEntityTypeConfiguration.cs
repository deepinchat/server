using Deepin.Domain.Emails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Deepin.Infrastructure.EntityTypeConfigurations;

public class EmailEntityTypeConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.ToTable("emails");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(s => s.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").ValueGeneratedOnAdd().HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
        builder.Property(s => s.From).HasColumnName("from").IsRequired();
        builder.Property(s => s.To).HasColumnName("to").IsRequired();
        builder.Property(s => s.Subject).HasColumnName("subject").IsRequired();
        builder.Property(s => s.Body).HasColumnName("body").IsRequired();
        builder.Property(s => s.CC).HasColumnName("cc");
        builder.Property(s => s.IsBodyHtml).HasColumnName("is_body_html").IsRequired();
    }
}
