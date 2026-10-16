using PropertyRental.Application.Common.Models;

namespace PropertyRental.API.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (!result.IsSuccess)
            return Results.Json(
                new { errors = result.Errors },
                statusCode: result.StatusCode);

        return Results.Json(
            result.Data,
            statusCode: result.StatusCode);
    }

    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
            return Results.StatusCode(result.StatusCode);

        return Results.Json(
            new { errors = result.Errors },
            statusCode: result.StatusCode);
    }
}