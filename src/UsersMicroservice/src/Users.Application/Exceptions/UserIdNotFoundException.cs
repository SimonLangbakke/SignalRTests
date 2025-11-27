namespace Users.Application.Exceptions;

public class UserIdNotFoundException : ApplicationException
{
    public UserIdNotFoundException(Guid id) 
        : base($"User with ID: {id} not found.") { }
    
}