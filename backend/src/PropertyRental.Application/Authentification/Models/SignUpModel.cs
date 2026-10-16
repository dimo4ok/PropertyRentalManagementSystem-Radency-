using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.Application.Authentification.Models
{
    public record SignUpModel(
        string UserName,
        string Email,
        string FirstName,
        string LastName,
        string Password,
        string? PhoneNumber,
        UserRole UserRole);
}