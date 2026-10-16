using PropertyRental.Application.Authentification.Models;

namespace PropertyRental.Application.Authentification.Interfaces;

public interface ITokenFactory
{
    AuthResponse GenerateAuthTokens(JwtPayloadModel payload);
}