namespace Events.Application.Dtos.Requests;

public record UpdateEventFoodRequest(
    string Name,
    string Ingredients,
    string AdditionalFoodItems
);