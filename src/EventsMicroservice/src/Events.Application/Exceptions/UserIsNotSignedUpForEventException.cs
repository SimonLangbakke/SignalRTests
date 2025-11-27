namespace Events.Application.Exceptions;

public class UserIsNotSignedUpForEventException : EventApplicationException
{
    public UserIsNotSignedUpForEventException(Guid userId, Guid eventId) 
        : base($"User: {userId} cannot withdraw from event: {eventId} since they're not signed up") { }
}