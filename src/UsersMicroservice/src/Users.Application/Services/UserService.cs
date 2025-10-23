using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Users.Application.Contracts.Repositories;
using Users.Application.Contracts.Services;
using Users.Application.Dtos;
using Users.Application.Dtos.Requests;
using Users.Application.Exceptions;
using Users.Application.Extensions;

namespace Users.Application.Services;

public class UserService(IUserRepository _userRepository, ITokenService _tokenService, IHttpContextAccessor _httpContextAccessor) : IUserService
{
    private readonly PasswordHasher<object> _passwordHasher = new();
    public async Task<Guid> CreateUserAsync(CreateUserRequestDto createUserDto)
    {
        var _validator = GetValidator<IValidator<CreateUserRequestDto>>();

        var validationResult = _validator.Validate(createUserDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToDictionary();
            throw new RequestValidationException(
               "Invalid user signup request",
               errors
           );
        }

        var hashedPassword = _passwordHasher.HashPassword(createUserDto.Email, createUserDto.Password);
        createUserDto.Password = hashedPassword;

        var user = await _userRepository.AddUser(createUserDto);
        return user.Id;
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        await _userRepository.DeleteUser(userId);
    }

    public Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<string> AuthenticateAndGenerateUserTokenAsync(LoginUserRequestDto requestDto)
    {
        var _validator = GetValidator<IValidator<LoginUserRequestDto>>();

        var validationResult = _validator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToDictionary();
            throw new RequestValidationException(
               "Invalid login request",
               errors
           );
        }

        var user = await _userRepository.GetUserByEmail(requestDto.Email);

        var verificationResult = _passwordHasher.VerifyHashedPassword(user.userDto.Email, user.hashedPassword, requestDto.Password);
        if (verificationResult == PasswordVerificationResult.Success)
        {
            return _tokenService.CreateToken(user.userDto.Id, user.userDto.Email, user.userDto.Name);
        }

        throw new WrongUserCredentialsException(requestDto.Email);
    }

    public Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateUserAsync(Guid userId, string name, string email)
    {
        throw new NotImplementedException();
    }

    public async Task DeactivateUserAsync(Guid id)
    {
        await _userRepository.DeactivateUser(id);
    }

    private T GetValidator<T>()
    {
        var scope = _httpContextAccessor.HttpContext.RequestServices.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
}
