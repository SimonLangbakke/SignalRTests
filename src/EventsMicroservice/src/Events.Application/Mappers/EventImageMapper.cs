
using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Mappers;

public static class EventImageMapper
{
    public static EventImageDto ToDto(this EventImage ei)
    {
        return new EventImageDto(
            ei.Id,
            ei.EventId,
            ei.Name,
            ei.RelativeUrl,
            ei.CreatedDate
        );
    }

    public static EventImage ToEntity(this AddEventImageRequestDto dto, Guid eventId)
    {
        return new EventImage
        {
            EventId = eventId,
            Name = dto.name,
            RelativeUrl = dto.url,
        };
    }
}