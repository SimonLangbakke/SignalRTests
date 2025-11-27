using Microsoft.EntityFrameworkCore;
using Events.Domain.Entities;
using Events.Infrastructure.EntityConfigurations;

namespace Events.Infrastructure.Context;

public class EventsDbContext : DbContext
{
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventParticipant> EventParticipants => Set<EventParticipant>();
    public DbSet<EventReview> EventReviews => Set<EventReview>();
    public DbSet<EventImage> EventImages => Set<EventImage>();
    public DbSet<EventFoodDetails> EventFoodDetails => Set<EventFoodDetails>();
    public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventEntityConfiguration());
        modelBuilder.ApplyConfiguration(new EventParticipantEntityConfiguration());
        modelBuilder.ApplyConfiguration(new EventFoodDetailsEntityConfiguration());
        modelBuilder.ApplyConfiguration(new EventImageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new EventReviewEntityConfiguration());

    }
}
