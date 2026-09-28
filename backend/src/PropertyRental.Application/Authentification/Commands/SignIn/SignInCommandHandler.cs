using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Authentification.Interfaces;
using PropertyRental.Application.Authentification.Models;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Domain.Entities.Identity;

namespace PropertyRental.Application.Authentification.Commands.SignIn;

public class SignInCommandHandler(
    ITokenFactory tokenFactory,
    UserManager<User> userManager,
    ILogger<SignInCommandHandler> logger) : ICommandHandler<SignInCommand, Result<AuthResponse>>
{
    private readonly ITokenFactory _tokenFactory = tokenFactory;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ILogger<SignInCommandHandler> _logger = logger;

    public async Task<Result<AuthResponse>> ExecuteAsync(SignInCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SignIn request received.");

        var user = await _userManager.FindByNameAsync(command.Model.UserName);
        if (user == null)
        {
            _logger.LogWarning("SignIn failed: User {UserName} not found. Error: {@Error}",
                command.Model.UserName, UserErrors.NotFound);
            return Result<AuthResponse>.Fail(UserErrors.NotFound);
        }

        var validPassword = await _userManager.CheckPasswordAsync(user, command.Model.Password);
        if (!validPassword)
        {
            _logger.LogWarning("SignIn failed: Invalid password for user {UserName}",
                command.Model.UserName);
            return Result<AuthResponse>.Fail(UserErrors.InvalidCredentials, StatusCodes.Status401Unauthorized);
        }

        var roleString = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        if (string.IsNullOrEmpty(roleString))
        {
            _logger.LogError("Critical Error: User {UserId} has no assigned identity roles", user.Id);
            return Result<AuthResponse>.Fail(UserErrors.RoleNotFound,
                StatusCodes.Status500InternalServerError);
        }

        if (!Enum.TryParse<UserRole>(roleString, true, out var userRole))
        {
            _logger.LogError("Critical Error: Role {Role} for User {UserId} is not a valid UserRole enum",
                roleString, user.Id);
            return Result<AuthResponse>.Fail(UserErrors.InvalidRole,
                StatusCodes.Status500InternalServerError);
        }

        var jwtPayloadModel = new JwtPayloadModel
        {
            UserId = user.Id,
            UserName = user.UserName!,
            Role = userRole
        };

        var response = _tokenFactory.GenerateAuthTokens(jwtPayloadModel);

        _logger.LogInformation(
            "SignIn completed successfully for user {UserName}. UserId: {userId}, Role: {Role}",
            user.UserName, response.UserId, response.Role);

        return Result<AuthResponse>.Success(response);
    }
}