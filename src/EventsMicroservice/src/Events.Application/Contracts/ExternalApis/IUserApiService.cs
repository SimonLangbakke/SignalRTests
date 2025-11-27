using Events.Application.Dtos;

namespace Events.Application.Contracts.ExternalApis;

public interface IUserApiService
{
    Task<IEnumerable<UserDto>> GetUsersByIds(IEnumerable<Guid> userIds);
}
