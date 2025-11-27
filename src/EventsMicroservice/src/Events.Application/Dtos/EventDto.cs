using Events.Domain.Entities;

namespace Events.Application.Dtos;

public record EventDto(
    Guid EventId,
    Guid UserId,
    string Name,
    string Description,
    int MaxAllowedParticipants,
    double PricePerSeat,
    int MinAllowedAge,
    int MaxAllowedAge,
    DateTime StartDate,
    DateTime? EndDate,
    DateTime ReservationEndDate,
    string ImageThumbnail,
    DateTime CreatedDate,
    bool IsActive,
    bool IsPublic,
    EventAddress EventAddress,
    EventFoodDetails EventFoodDetails,
    IEnumerable<EventParticipant> EventParticipants,
    IEnumerable<EventImage> EventImages
);