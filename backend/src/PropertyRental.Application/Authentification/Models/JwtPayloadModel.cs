using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.Application.Authentification.Models;

public class JwtPayloadModel
{
    public Guid UserId { get; init; }
    public string UserName { get; init; } = null!;
    public UserRole Role { get; init; }
}