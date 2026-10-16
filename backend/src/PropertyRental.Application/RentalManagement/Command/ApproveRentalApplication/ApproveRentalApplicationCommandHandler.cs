using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.Interfaces.Services;
using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Domain.Entities.PropertyEntities;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.RentalManagement.Command.ApproveRentalApplication;

public class ApproveRentalApplicationCommandHandler(
    IRentalApplicationRepository rentalApplicationRepository,
    IUnitRepository unitRepository,
    IUnitQueries unitQueries,
    ILeaseRepository leaseRepository,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork,
    ILogger<ApproveRentalApplicationCommandHandler> logger)
    : ICommandHandler<ApproveRentalApplicationCommand, Result>
{
    private readonly IRentalApplicationRepository _rentalApplicationRepository = rentalApplicationRepository;
    private readonly IUnitRepository _unitRepository = unitRepository;
    private readonly IUnitQueries _unitQueries = unitQueries;
    private readonly ILeaseRepository _leaseRepository = leaseRepository;
    private readonly IUserContextService _userContextService = userContextService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ApproveRentalApplicationCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(ApproveRentalApplicationCommand command, CancellationToken cancellationToken)
    {
        var user = _userContextService.Current;

        _logger.LogInformation(
            "Starting rental application approval. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, command.Id);

        var rentalApplication = await _rentalApplicationRepository.GetByIdWithStatusHistoriesAsync(command.Id, track: true, cancellationToken);
        if (rentalApplication is null)
        {
            _logger.LogWarning(
                "Rental application approval failed. Application not found. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
                user.UserId, command.Id);

            return Result.Fail(DomainErrors.NotFound(nameof(RentalApplication)));
        }

        if (rentalApplication.Status is not ApplicationStatus.Submitted)
        {
            _logger.LogWarning(
                "Rental application approval failed. Invalid status {Status}. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
                rentalApplication.Status, user.UserId, command.Id);

            return Result.Fail(RentalApplicationErrors.CannotApprove, StatusCodes.Status400BadRequest);
        }

        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
        var rowVersion = Convert.FromBase64String(command.Model.RowVersion);
        _rentalApplicationRepository.SetOriginalRowVersion(rentalApplication, rowVersion);

        var unit = await _unitRepository.GetByIdWithUpdateLockAsync(rentalApplication.UnitId, cancellationToken);
        if (unit is null)
        {
            _logger.LogWarning(
                "Rental application approval failed. Unit not found. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}, UnitId: {UnitId}",
                user.UserId, command.Id, rentalApplication.UnitId);

            return Result.Fail(DomainErrors.NotFound(nameof(Unit)));
        }

        var isAvailable = await _unitQueries.IsAvailableAsync(rentalApplication.UnitId, cancellationToken);
        if (!isAvailable)
        {
            _logger.LogWarning(
                "Rental application approval failed. Unit is not available. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}, UnitId: {UnitId}",
                user.UserId, command.Id, rentalApplication.UnitId);

            return Result.Fail(RentalApplicationErrors.UnitNotAvailable, StatusCodes.Status400BadRequest);
        }

        var now = DateTimeOffset.UtcNow;
        var previousStatus = rentalApplication.Status;
        var startDate = DateOnly.FromDateTime(now.UtcDateTime);
        var endDate = startDate.AddMonths(12);

        rentalApplication.Status = ApplicationStatus.Approved;
        rentalApplication.UpdatedAt = now;

        rentalApplication.StatusHistory.Add(new ApplicationStatusHistory
        {
            FromStatus = previousStatus,
            ToStatus = ApplicationStatus.Approved,
            ChangedAt = now,
            ChangedByUserId = user.UserId
        });

        var lease = new Lease
        {
            UnitId = unit.Id,
            UserId = rentalApplication.UserId,
            StartDate = startDate,
            EndDate = endDate,
            MonthlyRent = unit.MonthlyRent,
            CreatedAtUtc = now
        };

        await _leaseRepository.CreateAsync(lease, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        _logger.LogInformation(
            "Rental application approved successfully. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, rentalApplication.Id);

        return Result.Success();
    }
}