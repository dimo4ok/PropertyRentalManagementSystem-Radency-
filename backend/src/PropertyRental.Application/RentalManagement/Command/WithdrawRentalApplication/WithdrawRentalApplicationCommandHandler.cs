using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Application.Interfaces.Services;
using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.RentalManagement.Command.WithdrawRentalApplication;

public class WithdrawRentalApplicationCommandHandler(
    IRentalApplicationRepository rentalApplicationRepository,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork,
    ILogger<WithdrawRentalApplicationCommandHandler> logger)
    : ICommandHandler<WithdrawRentalApplicationCommand, Result>
{
    private readonly IRentalApplicationRepository _rentalApplicationRepository = rentalApplicationRepository;
    private readonly IUserContextService _userContextService = userContextService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<WithdrawRentalApplicationCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(WithdrawRentalApplicationCommand command,
        CancellationToken cancellationToken)
    {
        var user = _userContextService.Current;

        _logger.LogInformation(
            "Starting rental application withdrawal. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, command.Id);

        var rentalApplication = await _rentalApplicationRepository.GetByIdWithStatusHistoriesAsync(
            command.Id, track: true, cancellationToken);

        if (rentalApplication is null || rentalApplication.UserId != user.UserId)
        {
            _logger.LogWarning(
                "Rental application withdrawal failed. Application not found or does not belong to user. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
                user.UserId, command.Id);

            return Result.Fail(DomainErrors.NotFound(nameof(RentalApplication)));
        }

        if (rentalApplication.Status is not ApplicationStatus.Submitted and not ApplicationStatus.Returned and not ApplicationStatus.Draft)
        {
            _logger.LogWarning(
                "Rental application withdrawal failed. Invalid status {Status}. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
                rentalApplication.Status, user.UserId, command.Id);

            return Result.Fail(RentalApplicationErrors.CannotWithdraw, StatusCodes.Status400BadRequest);
        }

        var now = DateTimeOffset.UtcNow;
        var previousStatus = rentalApplication.Status;

        rentalApplication.Status = ApplicationStatus.Withdrawn;
        rentalApplication.UpdatedAt = now;

        rentalApplication.StatusHistory.Add(new ApplicationStatusHistory
        {
            FromStatus = previousStatus,
            ToStatus = ApplicationStatus.Withdrawn,
            ChangedAt = now,
            ChangedByUserId = user.UserId
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Rental application withdrawn successfully. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, rentalApplication.Id);

        return Result.Success();
    }
}