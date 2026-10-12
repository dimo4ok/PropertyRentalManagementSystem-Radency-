using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Application.Interfaces.Services;
using PropertyRental.Application.RentalManagement.Extensions;
using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.RentalManagement.Command.UpdateRentalApplication;

public class UpdateRentalApplicationCommandHandler(
    IRentalApplicationRepository rentalApplicationRepository,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork,
    ILogger<UpdateRentalApplicationCommandHandler> logger)
    : ICommandHandler<UpdateRentalApplicationCommand, Result>
{
    private readonly IRentalApplicationRepository _rentalApplicationRepository = rentalApplicationRepository;
    private readonly IUserContextService _userContextService = userContextService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<UpdateRentalApplicationCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(UpdateRentalApplicationCommand command, CancellationToken cancellationToken)
    {
        var user = _userContextService.Current;

        _logger.LogInformation(
            "Starting rental application update. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, command.Id);

        var rentalApplication = await _rentalApplicationRepository.GetByIdWithResidencesAsync(command.Id, track: true, cancellationToken);
        if (rentalApplication is null || rentalApplication.UserId != user.UserId)
        {
            _logger.LogWarning(
                "Rental application update failed. Application not found or does not belong to user. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
                user.UserId, command.Id);

            return Result.Fail(DomainErrors.NotFound(nameof(RentalApplication)));
        }

        if (rentalApplication.Status is not ApplicationStatus.Draft and not ApplicationStatus.Returned)
        {
            _logger.LogWarning(
                "Rental application update failed. Invalid status {Status}. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
                rentalApplication.Status, user.UserId, command.Id);

            return Result.Fail(RentalApplicationErrors.CannotEdit, StatusCodes.Status400BadRequest);
        }

        rentalApplication.FullName = command.Model.FullName;
        rentalApplication.Phone = command.Model.Phone;
        rentalApplication.Email = command.Model.Email;
        rentalApplication.CurrentAddress = command.Model.CurrentAddress;
        rentalApplication.UpdatedAt = DateTimeOffset.UtcNow;

        rentalApplication.ResidenceHistory.Clear();

        foreach (var residence in command.Model.ResidenceHistory)
            rentalApplication.ResidenceHistory.Add(residence.ToEntity());

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Rental application updated successfully. UserId: {UserId}, RentalApplicationId: {RentalApplicationId}",
            user.UserId, rentalApplication.Id);
        return Result.Success();
    }
}