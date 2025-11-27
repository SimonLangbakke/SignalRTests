namespace Events.Application.Exceptions;

public class EventApplicationException : Exception 
{
    protected EventApplicationException(string message) : base(message) { }
}