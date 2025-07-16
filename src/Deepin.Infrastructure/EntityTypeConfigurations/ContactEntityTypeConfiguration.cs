using Deepin.Domain.ContactAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Deepin.Infrastructure.EntityTypeConfigurations;


public class ContactEntityTypeConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("contacts");

        builder.HasKey(c => c.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid").HasValueGenerator(typeof(SequentialGuidValueGenerator));

        builder.Property(c => c.UserId).HasColumnName("user_id").HasColumnType("uuid");
        builder.Property(c => c.Name).HasColumnName("name");
        builder.Property(c => c.Notes).HasColumnName("notes");
        builder.Property(c => c.IsBlocked).HasColumnName("is_blocked").HasDefaultValue(false);
        builder.Property(c => c.IsStarred).HasColumnName("is_starred").HasDefaultValue(false);
        builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasColumnType("uuid").IsRequired();

        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
    }
}
