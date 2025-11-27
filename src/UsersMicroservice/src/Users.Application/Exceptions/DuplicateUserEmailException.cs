namespace Users.Application.Exceptions;

public class DuplicateUserEmailException : ApplicationException
{
    public DuplicateUserEmailException(string email) 
        : base($"User with email {email} already exists.") { }
}