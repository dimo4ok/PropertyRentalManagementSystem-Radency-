using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.Infrastructure.Services;

public class UserContextService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<UserContextService> logger) : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ILogger<UserContextService> _logger = logger;

    public UserContext Current
    {
        get
        {
            var httpContextUser = _httpContextAccessor.HttpContext?.User;

            return httpContextUser is { Identity.IsAuthenticated: true }
                ? new UserContext(
                    Guid.Parse(httpContextUser.FindFirstValue(ClaimTypes.NameIdentifier)!),
                    httpContextUser.FindFirstValue(ClaimTypes.Name)!,
                    Enum.Parse<UserRole>(httpContextUser.FindFirstValue(ClaimTypes.Role)!))
                : LogAndThrow();
        }
    }

    private UserContext LogAndThrow()
    {
        _logger.LogError("Access denied: User is not authenticated or HttpContext is null.");
        throw new UnauthorizedAccessException("User not authenticated");
    }
}