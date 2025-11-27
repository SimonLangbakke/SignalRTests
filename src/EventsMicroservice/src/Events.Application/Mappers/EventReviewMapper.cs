using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Mappers;

public static class EventReviewMapper
{
    public static EventReviewDto ToDto(this EventReview entity, IEnumerable<UserDto> users)
    {
        return new EventReviewDto(
            entity.Id,
            entity.EventId,
            entity.UserId,
            users.Where(x => x.Id == entity.UserId).FirstOrDefault()?.Name ?? string.Empty,
            entity.ReviewStars,
            entity.ReviewComment,
            entity.CreatedDate
        );
    }

    public static EventReview ToEntity(this CreateEventReviewRequestDto dto, Guid eventId, Guid userId)
    {
        return new EventReview
        {
            EventId = eventId,
            UserId = userId,
            ReviewStars = dto.Stars,
            ReviewComment = dto.Comment,
        };
    }
}