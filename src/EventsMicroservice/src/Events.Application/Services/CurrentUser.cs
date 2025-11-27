using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Events.Application.Services;

public class CurrentUser(IHttpContextAccessor httpContext)
{
    public Guid UserId
    {
        get
        {
            var userIdClaim = httpContext.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value ??
                throw new UnauthorizedAccessException();
            return Guid.Parse(userIdClaim);
        }
    }

    public string Username
    {
        get
        {
            var userNameClaim = httpContext.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value ??
                throw new UnauthorizedAccessException();
            return userNameClaim;
        }
    }
}
