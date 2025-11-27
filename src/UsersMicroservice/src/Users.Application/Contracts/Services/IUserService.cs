using Users.Application.Dtos;
using Users.Application.Dtos.Requests;

namespace Users.Application.Contracts.Services;

public interface IUserService
{
    Task<Guid> CreateUserAsync(CreateUserRequestDto createUserDto);
    Task DeleteUserAsync(Guid userId);
    Task<bool> UpdateUserAsync(Guid userId, string name, string email);
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<IEnumerable<UserDto>?> GetMultipleUsersByIdsAsync(IEnumerable<Guid> userIds);
    Task<string> AuthenticateAndGenerateUserTokenAsync(LoginUserRequestDto requestDto);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task DeactivateUserAsync(Guid id);
}
