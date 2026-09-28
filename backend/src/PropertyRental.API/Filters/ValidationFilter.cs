using FluentValidation;
using PropertyRental.Application.Common.Extensions;
using PropertyRental.Application.Common.Models;

namespace PropertyRental.API.Filters;

public class ValidationFilter<T>(IServiceProvider serviceProvider) : IEndpointFilter where T : class
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var model = context.Arguments.OfType<T>().FirstOrDefault();
        if (model == null)
            return await next(context);

        var validator = _serviceProvider.GetService<IValidator<T>>();
        if (validator == null)
            return await next(context);

        var validatorResult = await validator.ValidateAsync(model);
        if (!validatorResult.IsValid)
            return Results.BadRequest(Result.Fail(validatorResult.Errors.ToErrorList(), 400));

        return await next(context);
    }
}