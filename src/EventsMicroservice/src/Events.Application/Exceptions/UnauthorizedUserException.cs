namespace Events.Application.Exceptions;

public class UnauthorizedUserException : EventApplicationException
{
    public UnauthorizedUserException(Guid eventId, Guid eventUserId, Guid currentUserId)  
        : base($"User {currentUserId} does not have access to the event {eventId}, only {eventUserId} does") { }
}