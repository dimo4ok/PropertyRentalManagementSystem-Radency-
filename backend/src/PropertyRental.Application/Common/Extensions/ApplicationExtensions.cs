using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PropertyRental.Application.Authentification.Interfaces;
using PropertyRental.Application.Authentification.Services;
using PropertyRental.Application.Authentification.Validators;
using PropertyRental.Application.Common.Mediator;
using PropertyRental.Application.Common.Mediator.Abstractions;

namespace PropertyRental.Application.Common.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator.Mediator>().AddMediatorHandlers();
        services.AddScoped<ITokenFactory, TokenFactory>();

        //validator
        services.AddValidatorsFromAssembly(typeof(SignInModelValidator).Assembly);
        return services;
    }
}