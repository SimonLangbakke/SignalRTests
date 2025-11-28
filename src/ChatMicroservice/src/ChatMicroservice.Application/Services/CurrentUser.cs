using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ChatMicroservice.Application.Services;

public class CurrentUser(IHttpContextAccessor httpContext)
{
    public Guid UserId
    {
        get
        {
            var userIdClaim = httpContext.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User not authenticated");
            return Guid.Parse(userIdClaim);
        }
    }
}