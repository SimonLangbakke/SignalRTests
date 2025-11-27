using Events.Application.Contracts.ExternalApis;
using Events.Application.Dtos;
using System.Net.Http.Json;

namespace Events.Infrastructure.ExternalApis;

public class UserApiService(IHttpClientFactory httpClientFactory) : IUserApiService
{
    public async Task<IEnumerable<UserDto>> GetUsersByIds(IEnumerable<Guid> userIds)
    {
        HttpClient _httpClient = httpClientFactory.CreateClient("UserApiClient");

        var query = string.Join("&", userIds.Select(id => $"ids={id}"));

        var response = await _httpClient.GetAsync($"api/User/multiple-users?{query}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>() ?? Enumerable.Empty<UserDto>();
    }
}
