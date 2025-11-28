using ChatMicroservice.API.Hubs;
using ChatMicroservice.Application.Exceptions;
using ChatMicroservice.Application.ServiceCollectionExtensions;
using ChatMicroservice.Infrastructure.ServiceExtensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ChatMicroservice.API.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, (string Title, int StatusCode)> _exceptionsMap = new()
    {
        { typeof(ChatRoomNotFoundException), ("Chat room not found", StatusCodes.Status404NotFound) },
        { typeof(MessageNotFoundException), ("Message not found", StatusCodes.Status404NotFound) },
        { typeof(UnauthorizedChatAccessException), ("Unauthorized access", StatusCodes.Status403Forbidden) },
        { typeof(UnauthorizedAccessException), ("Unauthorized", StatusCodes.Status401Unauthorized) }
    };

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/problem+json";

        var type = exception.GetType();
        var (title, statusCode) = _exceptionsMap.TryGetValue(type, out var mapped)
            ? mapped
            : ("An unexpected error occurred", StatusCodes.Status500InternalServerError);

        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Title = title,
            Status = statusCode,
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred."
                : exception.Message,
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}

