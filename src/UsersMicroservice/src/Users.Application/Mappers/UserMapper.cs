using Users.Application.Dtos;
using Users.Domain.Entities;

namespace Users.Application.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(user.Id, user.Name, user.Email, user.Birthday, user.CreatedDate, user.IsActive);
    }
}
