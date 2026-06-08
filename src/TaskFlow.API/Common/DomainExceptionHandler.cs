using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.API.Common;

public class DomainExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DomainException domainException)
        {
            return false;
        }

        var (statusCode, title) = domainException switch
        {
            NotFoundException  => (StatusCodes.Status404NotFound,    "Bulunamadı"),
            ForbiddenException => (StatusCodes.Status403Forbidden,   "Yetkisiz işlem"),
            ConflictException  => (StatusCodes.Status409Conflict,    "Çakışma"),
            _                  => (StatusCodes.Status400BadRequest,  "Geçersiz istek")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title  = title,
            Detail = domainException.Message
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}