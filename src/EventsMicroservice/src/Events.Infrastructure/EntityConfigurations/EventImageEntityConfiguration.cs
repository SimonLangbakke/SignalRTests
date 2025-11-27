using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.EntityConfigurations;

public class EventImageEntityConfiguration : IEntityTypeConfiguration<EventImage>
{
    public void Configure(EntityTypeBuilder<EventImage> builder)
    {
        builder.HasKey(ei => ei.Id);
        builder.HasIndex(ei => ei.EventId);

        builder.Property(ei => ei.Name)
            .HasMaxLength(150);

        builder.Property(ei => ei.RelativeUrl)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(e => e.CreatedDate)
           .HasDefaultValueSql("CURRENT_TIMESTAMP")
           .ValueGeneratedOnAdd()
           .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
}
