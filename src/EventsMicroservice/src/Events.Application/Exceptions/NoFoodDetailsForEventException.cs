namespace Events.Application.Exceptions;

public class NoFoodDetailsForEventException : EventApplicationException
{
    public NoFoodDetailsForEventException(Guid eventId) 
        : base($"No food details exists for event with id {eventId}") { }
    
}