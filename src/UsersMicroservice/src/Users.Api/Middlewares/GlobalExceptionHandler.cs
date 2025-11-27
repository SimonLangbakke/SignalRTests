using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Exceptions;

namespace Users.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, (string Title, int StatusCode)> _exceptionsMap = new()
    {
        { typeof(DuplicateUserEmailException), ("Duplicate user email", StatusCodes.Status400BadRequest) },
        { typeof(UserIdNotFoundException), ("User not found", StatusCodes.Status404NotFound) },
        { typeof(WrongUserCredentialsException), ("Invalid user credentails", StatusCodes.Status401Unauthorized) },
        { typeof(RequestValidationException), ("Request validation errors", StatusCodes.Status400BadRequest) }
    };
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = exception switch
        {
            RequestValidationException validationException => CreateValidationProblemDetails(validationException, httpContext.Request.Path),
            _ => CreateProblemDetails(exception, httpContext.Request.Path),
        };

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        if (problemDetails is ValidationProblemDetails validationProblemDetails)
        {
            await httpContext.Response.WriteAsJsonAsync(validationProblemDetails, cancellationToken);
        }
        else
        {
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }

        return true;
    }

    private ValidationProblemDetails CreateValidationProblemDetails(RequestValidationException exception, string path)
    {
        _exceptionsMap.TryGetValue(typeof(RequestValidationException), out var type);
        return new ValidationProblemDetails
        {
            Instance = path,
            Title = type.Title,
            Status = type.StatusCode,
            Errors = exception.ValidationErrors,
            Detail = exception.Message
        };
    }

    private ProblemDetails CreateProblemDetails(Exception exception, string path)
    {
        var type = exception.GetType();

        var (title, statusCode) = _exceptionsMap.TryGetValue(type, out var mappedException) ? mappedException : ("An unexpected error occurred. Contact support if the problem persists.", StatusCodes.Status500InternalServerError);

        return new ProblemDetails
        {
            Instance = path,
            Title = title,
            Status = statusCode,
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred. Contact support if the problem persists."
                : exception.Message,
        };
    }
}
