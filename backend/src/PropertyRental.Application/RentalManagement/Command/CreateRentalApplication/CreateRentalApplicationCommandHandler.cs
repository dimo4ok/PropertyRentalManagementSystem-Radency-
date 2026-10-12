using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Application.Interfaces.Services;
using PropertyRental.Application.RentalManagement.Extensions;

namespace PropertyRental.Application.RentalManagement.Command.CreateRentalApplication;

public class CreateRentalApplicationCommandHandler(
    IRentalApplicationRepository rentalApplicationRepository,
    IRentalApplicationQueries rentalApplicationQueries,
    IUnitQueries unitQueries,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork,
    ILogger<CreateRentalApplicationCommandHandler> logger)
    : ICommandHandler<CreateRentalApplicationCommand, Result>
{
    private readonly IRentalApplicationRepository _rentalApplicationRepository = rentalApplicationRepository;
    private readonly IRentalApplicationQueries _rentalApplicationQueries = rentalApplicationQueries;
    private readonly IUnitQueries _unitQueries = unitQueries;
    private readonly IUserContextService _userContextService = userContextService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CreateRentalApplicationCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(CreateRentalApplicationCommand command, CancellationToken cancellationToken)
    {
        var user = _userContextService.Current;

        _logger.LogInformation("Starting rental application creation. UserId: {UserId}, UnitId: {UnitId}", user.UserId,
            command.Model.UnitId);

        if (await _rentalApplicationQueries.HasOpenApplicationForUnitAsync(user.UserId, command.Model.UnitId, cancellationToken))
        {
            _logger.LogWarning(
                "Rental application creation failed. Open application already exists. UserId: {UserId}, UnitId: {UnitId}",
                user.UserId, command.Model.UnitId);

            return Result.Fail(RentalApplicationErrors.OpenApplicationAlreadyExists, StatusCodes.Status400BadRequest);
        }

        if (!await _unitQueries.IsAvailableAsync(command.Model.UnitId, cancellationToken))
        {
            _logger.LogWarning(
                "Rental application creation failed. Unit is not available. UserId: {UserId}, UnitId: {UnitId}",
                user.UserId, command.Model.UnitId);
            return Result.Fail(RentalApplicationErrors.UnitNotAvailable, StatusCodes.Status400BadRequest);
        }

        var rentalApplication = command.Model.ToEntity(user.UserId);

        await _rentalApplicationRepository.CreateAsync(rentalApplication, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Rental application created successfully. RentalApplicationId: {RentalApplicationId}, UserId: {UserId}, UnitId: {UnitId}",
            rentalApplication.Id, user.UserId, command.Model.UnitId);
        return Result.Success();
    }
}