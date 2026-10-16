using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.Application.Authentification.Models;

public class AuthResponse
{
    public string AccessToken { get; init; } = null!;
    public DateTimeOffset Expires { get; init; }
    public Guid UserId { get; init; }
    public string UserName { get; init; } = null!;
    public UserRole Role { get; init; }
}