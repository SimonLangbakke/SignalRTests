namespace Events.Application.Exceptions;

public class CannotReviewException : EventApplicationException
{
    public CannotReviewException(Guid eventId) 
        : base($"Cannot comment on event {eventId}") { }

}