using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.EntityConfigurations;

public class EventReviewEntityConfiguration : IEntityTypeConfiguration<EventReview>
{
    public void Configure(EntityTypeBuilder<EventReview> builder)
    {
        builder.HasKey(er => er.Id);

        builder.Property(er => er.UserId)
            .IsRequired();

        builder.Property(er => er.ReviewStars)
            .IsRequired();

        builder.Property(er => er.ReviewComment).HasMaxLength(500);

        builder.Property(e => e.CreatedDate)
        .HasDefaultValueSql("CURRENT_TIMESTAMP")
        .ValueGeneratedOnAdd()
        .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
}
