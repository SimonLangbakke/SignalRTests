namespace Events.Application.Exceptions;

public class RequestValidationException : Exception
{
    public Dictionary<string, string[]> ValidationErrors { get; init; }
    public RequestValidationException(string message, Dictionary<string, string[]> errors)
       : base(message)
    {
        ValidationErrors = errors;
    }
}
