namespace Events.Application.Dtos;

public record EventImageDto(
    Guid ImageId,
    Guid EventId,
    string Name,
    string RelativeUrl,
    DateTime CreatedAt
);