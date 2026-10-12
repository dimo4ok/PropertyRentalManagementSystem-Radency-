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

namespace PropertyRental.Application.RentalManagement.Command.ReturnRentalApplication;

public class ReturnRentalApplicationCommandHandler(
    IRentalApplicationRepository rentalApplicationRepository,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork,
    ILogger<ReturnRentalApplicationCommandHandler> logger)
    : ICommandHandler<ReturnRentalApplicationCommand, Result>
{
    private readonly IRentalApplicationRepository _rentalApplicationRepository = rentalApplicationRepository;
    private readonly IUserContextService _userContextService = userContextService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ReturnRentalApplicationCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(ReturnRentalApplicationCommand command,
        CancellationToken cancellationToken)
    {
        var user = _userContextService.Current;

        _logger.LogInformation(
            "Starting rental application return. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, command.Id);

        var rentalApplication = await _rentalApplicationRepository.GetByIdWithStatusHistoriesAsync(
            command.Id, track: true, cancellationToken);

        if (rentalApplication is null)
        {
            _logger.LogWarning(
                "Rental application return failed. Application not found. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
                user.UserId, command.Id);

            return Result.Fail(DomainErrors.NotFound(nameof(RentalApplication)));
        }

        if (rentalApplication.Status is not ApplicationStatus.Submitted)
        {
            _logger.LogWarning(
                "Rental application return failed. Invalid status {Status}. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
                rentalApplication.Status, user.UserId, command.Id);

            return Result.Fail(RentalApplicationErrors.CannotReturn, StatusCodes.Status400BadRequest);
        }

        var rowVersion = Convert.FromBase64String(command.ReviewModel.RowVersion);
        _rentalApplicationRepository.SetOriginalRowVersion(rentalApplication, rowVersion);

        var now = DateTimeOffset.UtcNow;

        rentalApplication.Status = ApplicationStatus.Returned;
        rentalApplication.UpdatedAt = now;

        rentalApplication.StatusHistory.Add(new ApplicationStatusHistory
        {
            FromStatus = ApplicationStatus.Submitted,
            ToStatus = ApplicationStatus.Returned,
            ChangedAt = now,
            Comment = command.ReviewModel.Comment,
            ChangedByUserId = user.UserId
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Rental application returned successfully. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, rentalApplication.Id);

        return Result.Success();
    }
}