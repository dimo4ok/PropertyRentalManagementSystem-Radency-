using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Interfaces;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Commands.UpdateProperty;

public class UpdatePropertyCommandHandler(
    IPropertyRepository propertyRepository,
    IPropertyQueries propertyQueries,
    IUnitOfWork unitOfWork,
    ILogger<UpdatePropertyCommandHandler> logger)
    : ICommandHandler<UpdatePropertyCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository;
    private readonly IPropertyQueries _propertyQueries = propertyQueries;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<UpdatePropertyCommandHandler> _logger = logger;

    public async Task<Result> ExecuteAsync(UpdatePropertyCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating property {PropertyId}.", command.Id);

        var property = await _propertyRepository.GetByIdAsync(command.Id, true, cancellationToken);
        if (property is null)
        {
            _logger.LogWarning("Property with id {PropertyId} was not found.", command.Id);
            return Result.Fail(DomainErrors.NotFound(nameof(Property)));
        }
        
        if (await _propertyQueries.ExistsAsync(command.Model.Name, command.Model.Address, cancellationToken, excludePropertyId: property.Id))
        {
            _logger.LogWarning(
                "Property with name {PropertyName} and address {PropertyAddress} already exists.", command.Model.Name, command.Model.Address);

            return Result.Fail(DomainErrors.AlreadyExists(nameof(Property)), StatusCodes.Status409Conflict);
        }

        var rowVersion = Convert.FromBase64String(command.Model.RowVersion);
        _propertyRepository.SetOriginalRowVersion(property, rowVersion);

        property.Name = command.Model.Name;
        property.Address = command.Model.Address;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Property {PropertyId} updated successfully.", property.Id);
        return Result.Success();
    }
}