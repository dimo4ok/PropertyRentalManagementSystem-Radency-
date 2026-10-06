using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.PropertyManagement.Extensions;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Queries.GetPropertyById;

public class GetPropertyByIdQueryHandler(
    IPropertyRepository propertyRepository,
    ILogger<GetPropertyByIdQueryHandler> logger
) : IQueryHandler<GetPropertyByIdQuery, Result<PropertyModel>>
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository;
    private readonly ILogger<GetPropertyByIdQueryHandler> _logger = logger;

    public async Task<Result<PropertyModel>> ExecuteAsync(GetPropertyByIdQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting property with id {PropertyId}.", query.Id);

        var property = await _propertyRepository.GetByIdWithUnitsAsync(query.Id, false, cancellationToken);
        if (property is null)
        {
            _logger.LogWarning("Property with id {PropertyId} was not found.", query.Id);
            return Result<PropertyModel>.Fail(DomainErrors.NotFound(nameof(Property)));
        }

        _logger.LogInformation("Property with id {PropertyId} retrieved successfully.", query.Id);
        return Result<PropertyModel>.Success(property.ToPropertyModel());
    }
}