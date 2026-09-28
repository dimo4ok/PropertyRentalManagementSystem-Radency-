namespace PropertyRental.API.Exceptions;

public interface IExceptionHandler
{
    Task InvokeAsync(HttpContext context);
}