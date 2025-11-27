namespace Events.Application.Exceptions;

public class UserIsNotTheRightAgeException : EventApplicationException
{
    public UserIsNotTheRightAgeException(Guid userId, int minAge, int maxAge)
        : base($"User with id {userId} needs to be in the age range [{minAge} - {maxAge}] to sign up for this event") { }

}