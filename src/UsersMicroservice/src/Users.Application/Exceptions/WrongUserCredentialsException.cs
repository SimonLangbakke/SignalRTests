namespace Users.Application.Exceptions;

public class WrongUserCredentialsException : ApplicationException
{
    public WrongUserCredentialsException(string email)
        : base($"invalid credentials: {email}") { }

}