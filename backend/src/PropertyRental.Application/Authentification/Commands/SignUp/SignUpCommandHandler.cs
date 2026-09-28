using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Authentification.Interfaces;
using PropertyRental.Application.Authentification.Models;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Extensions;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Domain.Entities.Identity;

namespace PropertyRental.Application.Authentification.Commands.SignUp;

public class SignUpCommandHandler(
    UserManager<User> userManager,
    ITokenFactory tokenFactory,
    IUnitOfWork unitOfWork,
    ILogger<SignUpCommandHandler> logger) : ICommandHandler<SignUpCommand, Result<AuthResponse>>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly ITokenFactory _tokenFactory = tokenFactory;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<SignUpCommandHandler> _logger = logger;

    public async Task<Result<AuthResponse>> ExecuteAsync(SignUpCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SignUp attempt for Email: {Email}, UserName: {UserName}, Role: {Role}",
            command.Model.Email, command.Model.UserName, command.Model.UserRole);

        var user = new User
        {
            UserName = command.Model.UserName,
            Email = command.Model.Email,
            PhoneNumber = command.Model.PhoneNumber,
            FirstName = command.Model.FirstName,
            LastName = command.Model.LastName,
        };

        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
        try
        {
            var createUserResult = await _userManager.CreateAsync(user, command.Model.Password);
            if (!createUserResult.Succeeded)
            {
                _logger.LogWarning("SignUp failed: Identity creation errors for {Email}. Errors: {@IdentityErrors}",
                    command.Model.Email, createUserResult.Errors);
                return Result<AuthResponse>.Fail(createUserResult.Errors.ToErrorList(),
                    StatusCodes.Status400BadRequest);
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, command.Model.UserRole.ToString());
            if (!addRoleResult.Succeeded)
            {
                _logger.LogError("SignUp failed: Could not assign role {Role} to user {Email}. Errors: {@RoleErrors}",
                    command.Model.UserRole.ToString(), command.Model.Email, addRoleResult.Errors);

                await transaction.RollbackAsync(cancellationToken);
                return Result<AuthResponse>.Fail(addRoleResult.Errors.ToErrorList(),
                    StatusCodes.Status400BadRequest);
            }

            var jwtPayloadModel = new JwtPayloadModel
            {
                UserId = user.Id,
                UserName = command.Model.UserName,
                Role = command.Model.UserRole
            };

            var response = _tokenFactory.GenerateAuthTokens(jwtPayloadModel);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "User registration completed successfully. UserName: {UserName}, UserId: {UserId}, Role: {Role}.",
                command.Model.UserName, user.Id, command.Model.UserRole);

            return Result<AuthResponse>.Success(response, StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignUp transaction failed for {Email} due to an unexpected error",
                command.Model.Email);

            await transaction.RollbackAsync(cancellationToken);
            return Result<AuthResponse>.Fail(SystemErrors.TransactionFailed,
                StatusCodes.Status500InternalServerError);
        }
    }
}