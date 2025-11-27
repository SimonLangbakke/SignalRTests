namespace Events.Domain.Entities;

public class EventFoodDetails
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public List<string> DietaryStyles { get; set; } = [];
    public List<string> Allergens { get; set; } = [];
    public string? Name { get; set; }
    public string? Ingredients { get; set; }
    public string? AdditionalFoodItems { get; set; }
}