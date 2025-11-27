namespace Events.Application.Dtos.Requests;

public record CreateEventReviewRequestDto(
    Int16 Stars,
    string Comment
);