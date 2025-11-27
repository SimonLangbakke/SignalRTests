namespace Events.Application.Exceptions;

public class ReviewIdNotFoundException : EventApplicationException
{
    public ReviewIdNotFoundException(Guid id) : base($"Comment with id {id} not found.") { }
    
}