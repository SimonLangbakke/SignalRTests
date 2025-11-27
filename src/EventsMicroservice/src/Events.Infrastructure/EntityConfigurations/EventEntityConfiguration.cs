using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Events.Domain.Entities;

namespace Events.Infrastructure.EntityConfigurations;

public class EventEntityConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.EventId);
        builder.Property(e => e.EventId);

        builder.Property(e => e.UserId).IsRequired();
        builder.HasIndex(e => e.UserId);

        builder.Property(e => e.Name).HasMaxLength(150).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(150).IsRequired();
        builder.Property(e => e.ImageThumbnail).HasMaxLength(150).IsRequired();

        builder.Property(e => e.MaxAllowedParticipants).IsRequired();
        builder.Property(e => e.MinAllowedAge).IsRequired();
        builder.Property(e => e.MaxAllowedAge).IsRequired();

        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.ReservationEndDate).IsRequired();

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(e => e.IsActive);
        
        builder.OwnsOne(e => e.EventAddress, address =>
        {
            address.Property(a => a.StreetAddress).HasMaxLength(256).IsRequired();
            address.Property(a => a.PostalCode).HasMaxLength(32).IsRequired();
            address.Property(a => a.City).HasMaxLength(128).IsRequired();
            address.Property(a => a.Region).HasMaxLength(128).IsRequired();

            address.WithOwner();
        });

        builder
            .HasOne(e => e.EventFoodDetails)
            .WithOne()
            .HasForeignKey<EventFoodDetails>(fd => fd.EventId)
            .IsRequired();
        
        builder
            .HasMany(e => e.EventParticipants)
            .WithOne()
            .HasForeignKey(ep => ep.EventId)
            .IsRequired();

        builder
            .HasMany(e => e.EventReviews)
            .WithOne()
            .HasForeignKey(ec => ec.EventId)
            .IsRequired();
        
        builder 
            .HasMany(e => e.EventImages)
            .WithOne()
            .HasForeignKey(ei => ei.EventId)
            .IsRequired();
    }
}