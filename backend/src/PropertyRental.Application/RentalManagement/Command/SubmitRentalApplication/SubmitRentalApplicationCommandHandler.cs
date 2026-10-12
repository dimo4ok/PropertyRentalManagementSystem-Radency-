using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Application.Interfaces.Services;
using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.RentalManagement.Command.SubmitRentalApplication;

public class SubmitRentalApplicationCommandHandler(
    IRentalApplicationRepository rentalApplicationRepository,
    IUnitQueries unitQueries,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork,
    ILogger<SubmitRentalApplicationCommandHandler> logger)
    : ICommandHandler<SubmitRentalApplicationCommand, Result>
{
    private readonly IRentalApplicationRepository _rentalApplicationRepository = rentalApplicationRepository;
    private readonly IUnitQueries _unitQueries = unitQueries;
    private readonly IUserContextService _userContextService = userContextService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<SubmitRentalApplicationCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(SubmitRentalApplicationCommand command, CancellationToken cancellationToken)
    {
        var user = _userContextService.Current;
        _logger.LogInformation(
            "Starting rental application submission. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, command.Id);

        var rentalApplication = await _rentalApplicationRepository.GetByIdWithStatusHistoriesAsync(command.Id, track: true, cancellationToken);
        if (rentalApplication is null || rentalApplication.UserId != user.UserId)
        {
            _logger.LogWarning(
                "Rental application submission failed. Application not found or does not belong to user. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
                user.UserId, command.Id);

            return Result.Fail(DomainErrors.NotFound(nameof(RentalApplication)));
        }

        if (rentalApplication.Status is not ApplicationStatus.Draft and not ApplicationStatus.Returned)
        {
            _logger.LogWarning(
                "Rental application submission failed. Invalid status {Status}. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
                rentalApplication.Status, user.UserId, command.Id);

            return Result.Fail(RentalApplicationErrors.CannotSubmit, StatusCodes.Status400BadRequest);
        }
        
        if (!await _unitQueries.IsAvailableAsync(rentalApplication.UnitId, cancellationToken))
        {
            _logger.LogWarning(
                "Rental application submission failed. Unit is not available. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}, UnitId: {UnitId}",
                user.UserId, command.Id, rentalApplication.UnitId);

            return Result.Fail(RentalApplicationErrors.UnitNotAvailable, StatusCodes.Status400BadRequest);
        }

        var now = DateTimeOffset.UtcNow;
        var previousStatus = rentalApplication.Status;

        rentalApplication.Status = ApplicationStatus.Submitted;
        rentalApplication.UpdatedAt = now;

        rentalApplication.StatusHistory.Add(new ApplicationStatusHistory
        {
            FromStatus = previousStatus,
            ToStatus = ApplicationStatus.Submitted,
            ChangedAt = now,
            ChangedByUserId = user.UserId
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Rental application submitted successfully. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, rentalApplication.Id);

        return Result.Success();
    }
}