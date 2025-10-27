using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace Chat.Application.Services;

public class UserIntegrationService : IUserIntegrationService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<UserIntegrationService> _logger;

    private readonly SemaphoreSlim _semaphore = new(3); // Limit concurrent calls to UsersAPI to 3

    public UserIntegrationService(
        HttpClient httpClient,
        IMemoryCache cache,
        ILogger<UserIntegrationService> logger)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
    }

    public async Task<string?> GetUserNameAsync(Guid userId)
    {
        // Some caching
        var cacheKey = $"username_{userId}";

        if (_cache.TryGetValue<string>(cacheKey, out var cachedName))
        {
            _logger.LogDebug("User name for {UserId} found in cache", userId);
            return cachedName;
        }

        // Limit concurrent calls to UsersAPI
        await _semaphore.WaitAsync();
        try
        {
            // Double-check cache after acquiring semaphore (another request might have fetched it)
            if (_cache.TryGetValue<string>(cacheKey, out cachedName))
            {
                return cachedName;
            }

            var response = await _httpClient.GetAsync($"api/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var userDto = await response.Content.ReadFromJsonAsync<UserDto>();
                if (userDto != null)
                {
                    var userName = userDto.Name;

                    // Cache 10 minutes for now
                    // OK for now, but could be improved with distributed cache if needed
                    _cache.Set(cacheKey, userName, TimeSpan.FromMinutes(10));

                    _logger.LogInformation(
                        "Successfully fetched and cached user name for {UserId}",
                        userId);

                    return userName;
                }
            }
            else
            {
                _logger.LogWarning(
                    "Failed fetching from UsersAPI",
                    userId);
            }

            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex,
                "Failed to reach UsersAPI when fetching user {UserId}",
                userId);
            return null;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex,
                "Timeout calling UsersAPI for user {UserId}",
                userId);
            return null;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Dictionary<Guid, string>> GetUserNamesAsync(List<Guid> userIds)
    {
        
        var result = new ConcurrentDictionary<Guid, string>();

        // Check cache for all users
        var uncachedIds = new List<Guid>();
        foreach (var userId in userIds)
        {
            var cacheKey = $"username_{userId}";
            if (_cache.TryGetValue<string>(cacheKey, out var cachedName))
            {
                result[userId] = cachedName;
            }
            else
            {
                uncachedIds.Add(userId);
            }
        }

        if (!uncachedIds.Any())
        {
            return new Dictionary<Guid, string>(result);
        }

        _logger.LogInformation(
            "Need to fetch {Count} uncached users from UsersAPI",
            uncachedIds.Count);

        // Uncached fetches
        var tasks = uncachedIds.Select(async userId =>
        {
            var name = await GetUserNameAsync(userId);
            if (name != null)
            {
                result[userId] = name;
            }
            else
            {
                // Random temp name
                result[userId] = $"User_{userId.ToString().Substring(0, 8)}";
            }
        });

        // Process in batches to limit concurrent load
        const int batchSize = 3;
        for (int i = 0; i < tasks.Count(); i += batchSize)
        {
            var batch = tasks.Skip(i).Take(batchSize);
            await Task.WhenAll(batch);
        }

        return new Dictionary<Guid, string>(result);
    }
}