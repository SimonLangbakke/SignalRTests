namespace Events.Domain.Entities;

public class EventReview
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public Int16 ReviewStars { get; set; }
    public string ReviewComment { get; set; } = "";
    public DateTime CreatedDate { get; set; }
}