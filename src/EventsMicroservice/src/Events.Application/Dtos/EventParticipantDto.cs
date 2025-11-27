using Events.Domain.Enums;

namespace Events.Application.Dtos;

public record EventParticipantDto(
    Guid Id,
    Guid EventId,
    Guid UserId,
    DateTime CreatedDate,
    ParticipantStatus ParticipantStatus,
    PaymentStatus PaymentStatus,
    string PaymentIntetId
);