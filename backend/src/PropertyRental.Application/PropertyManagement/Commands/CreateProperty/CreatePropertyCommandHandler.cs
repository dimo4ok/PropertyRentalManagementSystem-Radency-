using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.PropertyManagement.Extensions;
using PropertyRental.Application.PropertyManagement.Interfaces;

namespace PropertyRental.Application.PropertyManagement.Commands.CreateProperty;

public class CreatePropertyCommandHandler(
    IPropertyRepository propertyRepository,
    IUnitTypeService unitTypeService,
    IUnitOfWork unitOfWork,
    ILogger<CreatePropertyCommandHandler> logger)
    : ICommandHandler<CreatePropertyCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository;
    private readonly IUnitTypeService _unitTypeService = unitTypeService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CreatePropertyCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(CreatePropertyCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating property {PropertyName} with {UnitsCount} units.",
            command.Model.Name, command.Model.Units.Count);

        var unitTypeIds = command.Model.Units.Select(x => x.UnitTypeId);

        var unitTypeResult = await _unitTypeService.CheckCanBeUsedAsync(unitTypeIds, cancellationToken);
        if (!unitTypeResult.IsSuccess)
        {
            _logger.LogWarning("Property creation failed because one or more UnitTypes are invalid or inactive.");
            return unitTypeResult;
        }

        var property = command.Model.ToEntity();

        await _propertyRepository.CreateAsync(property, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Property created successfully. PropertyId: {PropertyId}.", property.Id);
        return Result.Success(StatusCodes.Status201Created);
    }
}