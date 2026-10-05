using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Application.PropertyManagement.Services;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Commands.UpdateProperty;

public class UpdatePropertyCommandHandler(
    IPropertyRepository propertyRepository,
    IUnitService unitService,
    IUnitOfWork unitOfWork,
    ILogger<UpdatePropertyCommandHandler> logger)
    : ICommandHandler<UpdatePropertyCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository;
    private readonly IUnitService _unitService = unitService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<UpdatePropertyCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(UpdatePropertyCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating property {PropertyId}.", command.Model.Id);

        var property = await _propertyRepository.GetByIdAsync(command.Model.Id, cancellationToken, track: true);

        if (property is null)
        {
            _logger.LogWarning("Property with id {PropertyId} was not found.", command.Model.Id);
            return Result.Fail(DomainErrors.NotFound(nameof(Property)), StatusCodes.Status404NotFound);
        }

        var unitsResult = await _unitService.UpdateAsync(property, command.Model.Units, cancellationToken);
        if (!unitsResult.IsSuccess)
        {
            _logger.LogWarning("Property {PropertyId} update failed while updating units.", property.Id);
            return unitsResult;
        }

        property.Name = command.Model.Name;
        property.Address = command.Model.Address;
        property.UpdatedAt = DateTimeOffset.UtcNow;

        var rowVersion = Convert.FromBase64String(command.Model.RowVersion);
        _propertyRepository.SetOriginalRowVersion(property, rowVersion);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Property {PropertyId} updated successfully.", property.Id);
        return Result.Success();
    }
}