using PropertyRental.Application.Authentification.Models;

namespace PropertyRental.Application.Authentification.Interfaces;

public interface IJwtService
{
    string GenerateJwt(JwtPayloadModel model);
}