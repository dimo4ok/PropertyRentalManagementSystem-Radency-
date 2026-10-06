using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Commands.UpdateUnit;

public class UpdateUnitCommandHandler(
    IUnitRepository unitRepository,
    IUnitTypeService unitTypeService,
    IUnitOfWork unitOfWork,
    ILogger<UpdateUnitCommandHandler> logger)
    : ICommandHandler<UpdateUnitCommand, Result>
{
    private readonly IUnitRepository _unitRepository = unitRepository;
    private readonly IUnitTypeService _unitTypeService = unitTypeService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<UpdateUnitCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(UpdateUnitCommand command, CancellationToken cancellationToken)
    {
        var unit = await _unitRepository.GetByIdAsync(command.Id, true, cancellationToken);
        if (unit is null)
        {
            _logger.LogWarning("Unit {UnitId} was not found.", command.Id);
            return Result.Fail(DomainErrors.NotFound(nameof(Unit)), StatusCodes.Status404NotFound);
        }
        
        var rowVersion = Convert.FromBase64String(command.Model.RowVersion);
        _unitRepository.SetOriginalRowVersion(unit, rowVersion);

        if (unit.UnitTypeId != command.Model.UnitTypeId)
        {
            var unitTypeResult = await _unitTypeService.ValidateAsync(command.Model.UnitTypeId, cancellationToken);
            if (!unitTypeResult.IsSuccess)
                return unitTypeResult;
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