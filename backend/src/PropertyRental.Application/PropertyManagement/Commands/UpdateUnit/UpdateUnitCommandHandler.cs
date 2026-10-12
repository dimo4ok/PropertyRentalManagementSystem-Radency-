using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Application.Interfaces.Services;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Commands.UpdateUnit;

public class UpdateUnitCommandHandler(
    IUnitRepository unitRepository,
    IUnitTypeService unitTypeService,
    ILeaseQueries leaseQueries,
    IRentalApplicationQueries rentalApplicationQueries,
    IUnitQueries unitQueries,
    IUnitOfWork unitOfWork,
    ILogger<UpdateUnitCommandHandler> logger)
    : ICommandHandler<UpdateUnitCommand, Result>
{
    private readonly IUnitRepository _unitRepository = unitRepository;
    private readonly IUnitTypeService _unitTypeService = unitTypeService;
    private readonly ILeaseQueries _leaseQueries = leaseQueries;
    private readonly IRentalApplicationQueries _rentalApplicationQueries = rentalApplicationQueries;
    private readonly IUnitQueries _unitQueries = unitQueries;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<UpdateUnitCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(UpdateUnitCommand command, CancellationToken cancellationToken)
    {
        var unit = await _unitRepository.GetByIdWithUnitTypesAsync(command.Id, true, cancellationToken);
        if (unit is null)
        {
            _logger.LogWarning("Unit {UnitId} was not found.", command.Id);
            return Result.Fail(DomainErrors.NotFound(nameof(Unit)), StatusCodes.Status404NotFound);
        }
        
        var rowVersion = Convert.FromBase64String(command.Model.RowVersion);
        _unitRepository.SetOriginalRowVersion(unit, rowVersion);
        
        if (await _unitQueries.ExistsAsync(unit.PropertyId, command.Model.UnitNumber, cancellationToken, excludeUnitId: unit.Id))
        {
            _logger.LogWarning("Unit {UnitNumber} already exists for property {PropertyId}.", command.Model.UnitNumber, unit.PropertyId);

            return Result.Fail(DomainErrors.AlreadyExists(nameof(Unit)), StatusCodes.Status409Conflict);
        }

        if (unit.UnitTypeId != command.Model.UnitTypeId)
        {
            var unitTypeResult = await _unitTypeService.ValidateAsync(command.Model.UnitTypeId, cancellationToken);
            if (!unitTypeResult.IsSuccess)
            {
                _logger.LogWarning("Unit type {UnitTypeId} cannot be assigned to unit {UnitId}.", command.Model.UnitTypeId, unit.Id); 
                return unitTypeResult;
            }
        }

        if (await _leaseQueries.HasActiveLeaseAsync(unit.Id, cancellationToken))
        {
            _logger.LogWarning("Unit {UnitId} cannot be updated because it has an active lease.", unit.Id);

            return Result.Fail(UnitErrors.UnitHasActiveLease, StatusCodes.Status409Conflict);
        }

        if (await _rentalApplicationQueries.HasSubmittedOrApprovedApplicationForUnitAsync(unit.Id, cancellationToken))
        {
            _logger.LogWarning("Unit {UnitId} cannot be updated because it has a submitted or approved application.", unit.Id);

            return Result.Fail(UnitErrors.UnitHasSubmittedOrApprovedApplication, StatusCodes.Status409Conflict);
        }

        unit.UnitNumber = command.Model.UnitNumber;
        unit.Bedrooms = command.Model.Bedrooms;     
        unit.MonthlyRent = command.Model.MonthlyRent;
        unit.UnitTypeId = command.Model.UnitTypeId;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Unit {UnitId} updated successfully.", unit.Id);
        return Result.Success();
    }
}