using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Exceptions;

namespace Users.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/problem+json";
        ProblemDetails problemDetails = new();
        ValidationProblemDetails validationProblemDetails = new();

        problemDetails.Instance = httpContext.Request.Path;
        problemDetails.Detail = exception.Message;

        validationProblemDetails.Instance = httpContext.Request.Path;
        validationProblemDetails.Detail = exception.Message;

        switch (exception)
        {
            case DuplicateUserEmailException:
                problemDetails.Title = "Duplicate user email";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                break;
            case UserIdNotFoundException:
                problemDetails.Title = "User not found";
                problemDetails.Status = StatusCodes.Status404NotFound;
                break;
            case WrongUserCredentialsException:
                problemDetails.Title = "Invalid user crendentials";
                problemDetails.Status = StatusCodes.Status404NotFound;
                break;
            case RequestValidationException:
                var requestValidationException = (RequestValidationException)exception;
                validationProblemDetails.Title = "Request Validation Errors";
                validationProblemDetails.Status = StatusCodes.Status400BadRequest;
                validationProblemDetails.Errors = requestValidationException.ValidationErrors;
                break;
            default:
                problemDetails.Title = "An unexpected error occurred.";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Detail = "An unexpected error occurred. Contact support if the problem persists.";
                break;
        }
        if (exception is RequestValidationException)
        {
            httpContext.Response.StatusCode = validationProblemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(validationProblemDetails, cancellationToken);
        }
        else
        {
            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }

        return true;
    }
}
