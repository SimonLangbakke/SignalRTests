using Users.Application.Dtos;
using Users.Application.Dtos.Requests;
using Users.Domain.Entities;

namespace Users.Application.Contracts.Repositories;

public interface IUserRepository
{
    Task<UserDto> GetUserById(Guid id);
    Task<IEnumerable<UserDto>> GetMultipleUsersByIds(IEnumerable<Guid> ids);
    Task<(UserDto userDto, string hashedPassword)> GetUserByEmail(string email);
    Task<List<UserDto>> GetAllUsers();
    Task<UserDto> AddUser(CreateUserRequestDto createUser);
    Task<UserDto> UpdateUser(Guid id, Action<User> op);
    Task DeleteUser(Guid id);
    Task DeactivateUser(Guid id);
}
