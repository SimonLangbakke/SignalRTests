namespace Events.Application.Dtos;

public record EventFoodDetailsDto(
    Guid Id,
    Guid EventId,
    string Name,
    string Ingredients,
    string AdditionalFoodItems
);