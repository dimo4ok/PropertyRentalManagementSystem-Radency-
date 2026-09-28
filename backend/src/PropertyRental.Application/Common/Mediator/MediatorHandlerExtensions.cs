using FitCoachHub.Application.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using PropertyRental.Application.Authentification.Commands.SignIn;
using PropertyRental.Application.Authentification.Commands.SignUp;
using PropertyRental.Application.Authentification.Models;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;

namespace PropertyRental.Application.Common.Mediator;

public static class MediatorHandlerExtensions
{
    public static IServiceCollection AddMediatorHandlers(this IServiceCollection services)
    {
        //auth
        services.AddScoped<ICommandHandler<SignUpCommand, Result<AuthResponse>>, SignUpCommandHandler>();
        services.AddScoped<ICommandHandler<SignInCommand, Result<AuthResponse>>, SignInCommandHandler>();

        return services;
    }
}