using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Domain.Exceptions;

namespace NotificationService.API.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        var status = exception switch
        {
            BadRequestException => (int)HttpStatusCode.BadRequest,
            InternalServerException => (int)HttpStatusCode.InternalServerError,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var problem = new ProblemDetails
        {
            Type = $"https://httpstatuses.io/{status}",
            Title = exception.GetType().Name,
            Status = status,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = status;
        
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problem), cancellationToken);
        return true;
    }

}