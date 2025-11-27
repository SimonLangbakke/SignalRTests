namespace Users.Application.Dtos.Requests;

public class LoginUserRequestDto
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
};
