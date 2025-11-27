namespace Events.Application.Exceptions;

public class RemoveEventParticipantException : EventApplicationException
{
    public RemoveEventParticipantException(Guid eventId, Guid userId)
        : base($"Cannot remove eventparticipant with id {userId} from event with id {eventId}") { }

}