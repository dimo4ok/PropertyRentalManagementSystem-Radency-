using PropertyRental.Application.Common.Models;

namespace PropertyRental.API.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result.Data);

        return Results.Json(
            new { errors = result.Errors },
            statusCode: result.StatusCode);
    }
}