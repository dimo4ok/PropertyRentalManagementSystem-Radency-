using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.API.Authentication;

public static class AuthorizationPolicies
{
    public const string PropertyManager = nameof(UserRole.PropertyManager);
    public const string Applicant = nameof(UserRole.Applicant);
}