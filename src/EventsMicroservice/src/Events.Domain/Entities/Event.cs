using Events.Domain.Enums;

namespace Events.Domain.Entities;

public class Event
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? GuestNotes { get; set; }
    public int MaxAllowedParticipants { get; set; }
    public double PricePerSeat { get; set; }
    public int MinAllowedAge { get; set; }
    public int MaxAllowedAge { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime ReservationEndDate { get; set; }
    public string ImageThumbnail { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublic { get; set; } = true;
    public EventAddress EventAddress { get; set; } = new();

    // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations
    public EventFoodDetails EventFoodDetails { get; set; }
    public ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
    public ICollection<EventImage> EventImages { get; set; } = new List<EventImage>();
    public ICollection<EventReview> EventReviews { get; set; } = new List<EventReview>();
}