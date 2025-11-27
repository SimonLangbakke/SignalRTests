using Events.Application.Dtos;
using Events.Domain.Entities;

namespace Events.Application.Mappers;

public static class EventParticipantMapper
{
    public static EventParticipantDto ToDto(this EventParticipant ep)
    {
        return new EventParticipantDto(
            ep.Id,
            ep.EventId,
            ep.UserId,
            ep.CreatedDate,
            ep.ParticipantStatus,
            ep.PaymentStatus,
            ep.PaymentIntentId
        );
    }
    
    public static EventParticipant ToEntity(this EventParticipantDto dto)
    {
        return new EventParticipant
        {
            Id = dto.Id,
            EventId = dto.EventId,
            UserId = dto.UserId,
            CreatedDate = dto.CreatedDate,
            ParticipantStatus = dto.ParticipantStatus,
            PaymentStatus = dto.PaymentStatus,
            PaymentIntentId = dto.PaymentIntetId
        };
    }
}