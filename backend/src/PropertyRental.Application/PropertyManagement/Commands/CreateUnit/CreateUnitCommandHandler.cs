using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.PropertyManagement.Extensions;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Commands.CreateUnit;

public class CreateUnitCommandHandler(
    IPropertyRepository propertyRepository,
    IUnitRepository unitRepository,
    IUnitTypeService unitTypeService,
    IUnitOfWork unitOfWork,
    ILogger<CreateUnitCommandHandler> logger
) : ICommandHandler<CreateUnitCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository;
    private readonly IUnitRepository _unitRepository = unitRepository;
    private readonly IUnitTypeService _unitTypeService = unitTypeService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CreateUnitCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(CreateUnitCommand command, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(command.PropertyId, false, cancellationToken);
        if (property is null)
        {
            _logger.LogWarning("Property {PropertyId} was not found.", command.PropertyId);
            return Result.Fail(DomainErrors.NotFound(nameof(Property)), StatusCodes.Status404NotFound);
        }

        var unitTypeResult = await _unitTypeService.ValidateAsync(command.Model.UnitTypeId, cancellationToken);
        if (!unitTypeResult.IsSuccess)
            return Result.Fail(unitTypeResult.Errors!, unitTypeResult.StatusCode);

        var unit = command.Model.ToEntity(property.Id);

        await _unitRepository.CreateAsync(unit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Unit {UnitId} created for property {PropertyId}.", unit.Id, property.Id);
        return Result.Success(StatusCodes.Status201Created);
    }
}