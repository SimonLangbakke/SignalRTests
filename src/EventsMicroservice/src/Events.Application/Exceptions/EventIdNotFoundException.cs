namespace Events.Application.Exceptions;

public class EventIdNotFoundException : EventApplicationException
{
    public EventIdNotFoundException(Guid id) 
        : base($"Event with id {id} not found.") { }
}