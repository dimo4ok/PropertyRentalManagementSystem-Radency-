using PropertyRental.API.Extensions;
using PropertyRental.API.Filters;
using PropertyRental.Application.Authentification.Commands.SignIn;
using PropertyRental.Application.Authentification.Commands.SignUp;
using PropertyRental.Application.Authentification.Models;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;

namespace PropertyRental.API.Authentication;

public static class AuthEndpoints
{
    private const string Auth = "1.Authentication";

    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(AuthRoutes.SignUp,
            async (
                SignUpModel model,
                IMediator mediator,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await mediator.ExecuteCommandAsync<SignUpCommand, Result<AuthResponse>>(
                    new SignUpCommand(model), cancellationToken);

                return response.ToHttpResult();
            }).AddEndpointFilter<ValidationFilter<SignUpModel>>().WithTags(Auth);

        app.MapPost(AuthRoutes.SignIn,
            async (
                SignInModel model,
                IMediator mediator,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await mediator.ExecuteCommandAsync<SignInCommand, Result<AuthResponse>>(
                    new SignInCommand(model), cancellationToken);

                return response.ToHttpResult();
            }).AddEndpointFilter<ValidationFilter<SignInModel>>().WithTags(Auth);
    }
}