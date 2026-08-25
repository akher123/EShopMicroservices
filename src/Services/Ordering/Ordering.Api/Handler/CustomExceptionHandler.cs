using BuildingBlocks.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.Api.Handler;

internal class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
            "Error Message: {exceptionMessage}, Time of occurrence {time}", exception.Message, DateTime.UtcNow);
        (string Detail, string Title, int SttuseCode) details = exception switch
        {
            InvalidServerException => (
                exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
            ValidationException => (
               exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            BadRequestException => (
                exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            NotFoundException => (
              exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status404NotFound
            ),
            _ => (
             exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError
            )

        };

        var pronlemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.SttuseCode,
            Instance = context.Request.Path
        };
        pronlemDetails.Extensions.Add("traceId", context.TraceIdentifier);
        if (exception is ValidationException validationExtension)
        {
            pronlemDetails.Extensions.Add("ValidationErrors", validationExtension.Errors);
        }
        await context.Response.WriteAsJsonAsync(pronlemDetails, cancellationToken);
        return true;
    }
}

