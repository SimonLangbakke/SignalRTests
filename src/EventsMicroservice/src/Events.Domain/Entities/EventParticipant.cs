using Events.Domain.Enums;

namespace Events.Domain.Entities;

public class EventParticipant
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public ParticipantStatus ParticipantStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    
    public string? PaymentIntentId { get; set; }
}