using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Extensions;
using PropertyRental.Application.Common.Models;

namespace PropertyRental.API.Exceptions;

public class GlobalExceptionHandler(
    RequestDelegate next,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled Exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (message, statusCode) = ex switch
        {
            DbUpdateConcurrencyException => (SystemErrors.ConcurrencyConflict, StatusCodes.Status409Conflict),
            _ => (ex.ToError(), StatusCodes.Status500InternalServerError)
        };

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(message);
    }
}