namespace Events.Application.Dtos;

public record EventReviewDto(
    Guid ReviewId,
    Guid EventId,
    Guid UserId,
    string Username,
    Int16 Stars,
    string Comment,
    DateTime CreatedAt
);