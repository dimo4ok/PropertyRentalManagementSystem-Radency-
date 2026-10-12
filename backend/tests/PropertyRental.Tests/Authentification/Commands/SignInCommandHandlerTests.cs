using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PropertyRental.Application.Authentification.Commands.SignIn;
using PropertyRental.Application.Authentification.Interfaces;
using PropertyRental.Application.Authentification.Models;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Domain.Entities.Identity;
using PropertyRental.Tests.Common;

namespace PropertyRental.Tests.Authentification.Commands;

public class SignInCommandHandlerTests
{
    private readonly ITokenFactory _tokenFactory = Substitute.For<ITokenFactory>();
    private readonly UserManager<User> _userManager = MockFactory.GetMockUserManager<User>();
    private readonly ILogger<SignInCommandHandler> _logger = Substitute.For<ILogger<SignInCommandHandler>>();

    private readonly SignInCommandHandler _sut;

    public SignInCommandHandlerTests()
    {
        _sut = new SignInCommandHandler(
            _tokenFactory,
            _userManager,
            _logger);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserNotFound_ReturnsFailResult()
    {
        // Arrange
        var command = AuthTestDataFactory.GetSignInCommand();

        _userManager
            .FindByNameAsync(Arg.Is(command.Model.UserName))
            .Returns((User?)null);

        // Act
        var result = await _sut.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(UserErrors.NotFound, result.Errors!);

        await _userManager.Received(1)
            .FindByNameAsync(Arg.Is(command.Model.UserName));

        await _userManager.DidNotReceive()
            .CheckPasswordAsync(Arg.Any<User>(), Arg.Any<string>());

        await _userManager.DidNotReceive()
            .GetRolesAsync(Arg.Any<User>());

        _tokenFactory.DidNotReceive()
            .GenerateAuthTokens(Arg.Any<JwtPayloadModel>());
    }

    [Fact]
    public async Task ExecuteAsync_WhenPasswordIsInvalid_ReturnsUnauthorizedResult()
    {
        // Arrange
        var command = AuthTestDataFactory.GetSignInCommand();
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = command.Model.UserName
        };

        _userManager
            .FindByNameAsync(Arg.Is(command.Model.UserName))
            .Returns(user);

        _userManager
            .CheckPasswordAsync(
                Arg.Is<User>(u => u.Id == user.Id),
                Arg.Is(command.Model.Password))
            .Returns(false);

        // Act
        var result = await _sut.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(UserErrors.InvalidCredentials, result.Errors!);
        Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);

        await _userManager.Received(1)
            .FindByNameAsync(Arg.Is(command.Model.UserName));

        await _userManager.Received(1)
            .CheckPasswordAsync(
                Arg.Is<User>(u => u.Id == user.Id),
                Arg.Is(command.Model.Password));

        await _userManager.DidNotReceive()
            .GetRolesAsync(Arg.Any<User>());

        _tokenFactory.DidNotReceive()
            .GenerateAuthTokens(Arg.Any<JwtPayloadModel>());
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserHasNoRole_ReturnsInternalServerErrorResult()
    {
        // Arrange
        var command = AuthTestDataFactory.GetSignInCommand();
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = command.Model.UserName
        };

        _userManager
            .FindByNameAsync(Arg.Is(command.Model.UserName))
            .Returns(user);

        _userManager
            .CheckPasswordAsync(
                Arg.Is<User>(u => u.Id == user.Id),
                Arg.Is(command.Model.Password))
            .Returns(true);

        _userManager
            .GetRolesAsync(Arg.Is<User>(u => u.Id == user.Id))
            .Returns(new List<string>());

        // Act
        var result = await _sut.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(UserErrors.RoleNotFound, result.Errors!);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

        await _userManager.Received(1)
            .GetRolesAsync(Arg.Is<User>(u => u.Id == user.Id));

        _tokenFactory.DidNotReceive()
            .GenerateAuthTokens(Arg.Any<JwtPayloadModel>());
    }

    [Fact]
    public async Task ExecuteAsync_WhenRoleIsInvalid_ReturnsInternalServerErrorResult()
    {
        // Arrange
        var command = AuthTestDataFactory.GetSignInCommand();
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = command.Model.UserName
        };

        _userManager
            .FindByNameAsync(Arg.Is(command.Model.UserName))
            .Returns(user);

        _userManager
            .CheckPasswordAsync(
                Arg.Is<User>(u => u.Id == user.Id),
                Arg.Is(command.Model.Password))
            .Returns(true);

        _userManager
            .GetRolesAsync(Arg.Is<User>(u => u.Id == user.Id))
            .Returns(new List<string> { "InvalidRole" });

        // Act
        var result = await _sut.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(UserErrors.InvalidRole, result.Errors!);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

        await _userManager.Received(1)
            .GetRolesAsync(Arg.Is<User>(u => u.Id == user.Id));

        _tokenFactory.DidNotReceive()
            .GenerateAuthTokens(Arg.Any<JwtPayloadModel>());
    }

    [Theory]
    [InlineData(UserRole.Applicant)]
    [InlineData(UserRole.PropertyManager)]
    public async Task ExecuteAsync_WhenCredentialsAreValid_ReturnsSuccessResult(
        UserRole role)
    {
        // Arrange
        var command = AuthTestDataFactory.GetSignInCommand();
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = command.Model.UserName
        };

        var authResponse = new AuthResponse
        {
            AccessToken = "test-access-token",
            Expires = DateTimeOffset.UtcNow.AddMinutes(15),
            UserId = user.Id,
            UserName = user.UserName!,
            Role = role
        };

        _userManager
            .FindByNameAsync(Arg.Is(command.Model.UserName))
            .Returns(user);

        _userManager
            .CheckPasswordAsync(
                Arg.Is<User>(u => u.Id == user.Id),
                Arg.Is(command.Model.Password))
            .Returns(true);

        _userManager
            .GetRolesAsync(Arg.Is<User>(u => u.Id == user.Id))
            .Returns(new List<string> { role.ToString() });

        _tokenFactory
            .GenerateAuthTokens(Arg.Is<JwtPayloadModel>(model =>
                model.UserId == user.Id &&
                model.UserName == user.UserName &&
                model.Role == role))
            .Returns(authResponse);

        // Act
        var result = await _sut.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(authResponse, result.Data);

        await _userManager.Received(1)
            .FindByNameAsync(Arg.Is(command.Model.UserName));

        await _userManager.Received(1)
            .CheckPasswordAsync(
                Arg.Is<User>(u => u.Id == user.Id),
                Arg.Is(command.Model.Password));

        await _userManager.Received(1)
            .GetRolesAsync(Arg.Is<User>(u => u.Id == user.Id));

        _tokenFactory.Received(1)
            .GenerateAuthTokens(Arg.Is<JwtPayloadModel>(model =>
                model.UserId == user.Id &&
                model.UserName == user.UserName &&
                model.Role == role));
    }
}