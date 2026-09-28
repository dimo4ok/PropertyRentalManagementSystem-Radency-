using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.API.Authentication;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAppAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.PropertyManager,
                policy => policy.RequireRole(nameof(UserRole.PropertyManager)));
            options.AddPolicy(AuthorizationPolicies.Applicant,
                policy => policy.RequireRole(nameof(UserRole.Applicant)));
        });

        return services;
    }
}