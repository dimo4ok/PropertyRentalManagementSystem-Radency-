using Microsoft.Extensions.Options;
using PropertyRental.Application.Authentification.Interfaces;
using PropertyRental.Application.Authentification.Models;
using PropertyRental.Application.Common.Configuration;

namespace PropertyRental.Application.Authentification.Services;

public class TokenFactory(
    IJwtService jwtService,
    IOptions<JwtOptions> jwtOptions) : ITokenFactory
{
    private readonly IJwtService _jwtService = jwtService;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public AuthResponse GenerateAuthTokens(JwtPayloadModel payload)
        => new()
        {
            AccessToken = _jwtService.GenerateJwt(payload),
            Expires = DateTimeOffset.UtcNow.AddSeconds(_jwtOptions.ExpirationSeconds),
            UserId = payload.UserId,
            UserName = payload.UserName,
            Role = payload.Role
        };
}