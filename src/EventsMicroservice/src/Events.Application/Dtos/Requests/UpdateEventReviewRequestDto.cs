namespace Events.Application.Dtos.Requests;

public record UpdateEventReviewRequestDto(
    Int16 Stars,
    string Comment
);
