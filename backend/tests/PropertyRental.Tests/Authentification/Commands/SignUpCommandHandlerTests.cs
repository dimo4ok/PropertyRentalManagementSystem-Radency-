using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PropertyRental.Application.Authentification.Commands.SignUp;
using PropertyRental.Application.Authentification.Interfaces;
using PropertyRental.Application.Authentification.Models;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Interfaces;
using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Domain.Entities.Identity;
using PropertyRental.Tests.Common;

namespace PropertyRental.Tests.Authentification.Commands;

public class SignUpCommandHandlerTests
{
    private readonly UserManager<User> _userManager = MockFactory.GetMockUserManager<User>();
    private readonly ITokenFactory _tokenFactory = Substitute.For<ITokenFactory>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ILogger<SignUpCommandHandler> _logger = Substitute.For<ILogger<SignUpCommandHandler>>();

    private readonly SignUpCommandHandler _sut;

    public SignUpCommandHandlerTests()
    {
        _sut = new SignUpCommandHandler(
            _userManager,
            _tokenFactory,
            _unitOfWork,
            _logger);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCreateUserFailed_ReturnsFailResult()
    {
        // Arrange
        var command = AuthTestDataFactory.GetSignUpCommand();
        var transaction = Substitute.For<IDbContextTransaction>();

        _unitOfWork
            .BeginTransactionAsync(cancellationToken: Arg.Any<CancellationToken>())
            .Returns(transaction);

        _userManager
            .CreateAsync(Arg.Any<User>(), Arg.Is(command.Model.Password))
            .Returns(IdentityResult.Failed(
                new IdentityError
                {
                    Code = "TestError",
                    Description = "User creation failed"
                }));

        // Act
        var result = await _sut.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors!);
        Assert.Equal("TestError", result.Errors![0].Code);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

        await _unitOfWork.Received(1)
            .BeginTransactionAsync(cancellationToken: Arg.Any<CancellationToken>());

        await _userManager.Received(1)
            .CreateAsync(Arg.Any<User>(), Arg.Is(command.Model.Password));

        await _userManager.DidNotReceive()
            .AddToRoleAsync(Arg.Any<User>(), Arg.Any<string>());

        _tokenFactory.DidNotReceive()
            .GenerateAuthTokens(Arg.Any<JwtPayloadModel>());

        await transaction.DidNotReceive()
            .RollbackAsync(Arg.Any<CancellationToken>());

        await transaction.DidNotReceive()
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_WhenAddRoleFailed_RollsBackTransactionAndReturnsFailResult()
    {
        // Arrange
        var command = AuthTestDataFactory.GetSignUpCommand();
        var transaction = Substitute.For<IDbContextTransaction>();

        _unitOfWork
            .BeginTransactionAsync(cancellationToken: Arg.Any<CancellationToken>())
            .Returns(transaction);

        _userManager
            .CreateAsync(Arg.Any<User>(), Arg.Is(command.Model.Password))
            .Returns(IdentityResult.Success);

        _userManager
            .AddToRoleAsync(Arg.Any<User>(), Arg.Is(command.Model.UserRole.ToString()))
            .Returns(IdentityResult.Failed(
                new IdentityError
                {
                    Code = "TestError",
                    Description = "Role assignment failed"
                }));

        // Act
        var result = await _sut.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors!);
        Assert.Equal("TestError", result.Errors![0].Code);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

        await _unitOfWork.Received(1)
            .BeginTransactionAsync(cancellationToken: Arg.Any<CancellationToken>());

        await _userManager.Received(1)
            .CreateAsync(Arg.Any<User>(), Arg.Is(command.Model.Password));

        await _userManager.Received(1)
            .AddToRoleAsync(Arg.Any<User>(), Arg.Is(command.Model.UserRole.ToString()));

        await transaction.Received(1)
            .RollbackAsync(Arg.Any<CancellationToken>());

        await transaction.DidNotReceive()
            .CommitAsync(Arg.Any<CancellationToken>());

        _tokenFactory.DidNotReceive()
            .GenerateAuthTokens(Arg.Any<JwtPayloadModel>());
    }

    [Fact]
    public async Task ExecuteAsync_WhenUnexpectedExceptionOccurs_RollsBackTransactionAndReturnsFailResult()
    {
        // Arrange
        var command = AuthTestDataFactory.GetSignUpCommand();
        var transaction = Substitute.For<IDbContextTransaction>();

        _unitOfWork
            .BeginTransactionAsync(cancellationToken: Arg.Any<CancellationToken>())
            .Returns(transaction);

        _userManager
            .CreateAsync(Arg.Any<User>(), Arg.Is(command.Model.Password))
            .Returns(IdentityResult.Success);

        _userManager
            .AddToRoleAsync(Arg.Any<User>(), Arg.Is(command.Model.UserRole.ToString()))
            .Returns(IdentityResult.Success);

        _tokenFactory
            .GenerateAuthTokens(Arg.Any<JwtPayloadModel>())
            .Returns(_ => throw new InvalidOperationException("Test exception"));

        // Act
        var result = await _sut.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors!);
        Assert.Equal(SystemErrors.TransactionFailed, result.Errors![0]);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

        await _unitOfWork.Received(1)
            .BeginTransactionAsync(cancellationToken: Arg.Any<CancellationToken>());

        await _userManager.Received(1)
            .CreateAsync(Arg.Any<User>(), Arg.Is(command.Model.Password));

        await _userManager.Received(1)
            .AddToRoleAsync(Arg.Any<User>(), Arg.Is(command.Model.UserRole.ToString()));

        await transaction.Received(1)
            .RollbackAsync(Arg.Any<CancellationToken>());

        await transaction.DidNotReceive()
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(UserRole.Applicant)]
    [InlineData(UserRole.PropertyManager)]
    public async Task ExecuteAsync_WhenEverythingIsValid_AssignsRequestedRole(
        UserRole role)
    {
        // Arrange
        var command = AuthTestDataFactory.GetSignUpCommand(role);
        var transaction = Substitute.For<IDbContextTransaction>();
        var authResponse = new AuthResponse();

        _unitOfWork
            .BeginTransactionAsync(cancellationToken: Arg.Any<CancellationToken>())
            .Returns(transaction);

        _userManager
            .CreateAsync(Arg.Any<User>(), Arg.Is(command.Model.Password))
            .Returns(IdentityResult.Success);

        _userManager
            .AddToRoleAsync(Arg.Any<User>(), Arg.Is(role.ToString()))
            .Returns(IdentityResult.Success);

        _tokenFactory
            .GenerateAuthTokens(Arg.Is<JwtPayloadModel>(model =>
                model.Role == role))
            .Returns(authResponse);

        // Act
        var result = await _sut.ExecuteAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        await _userManager.Received(1)
            .AddToRoleAsync(Arg.Any<User>(), role.ToString());

        _tokenFactory.Received(1)
            .GenerateAuthTokens(Arg.Is<JwtPayloadModel>(model =>
                model.Role == role));

        await transaction.Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }
}