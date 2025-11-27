namespace Users.Application.Contracts.Services;

public interface ITokenService
{
    string CreateToken(Guid userId, string email, string userName);
}
