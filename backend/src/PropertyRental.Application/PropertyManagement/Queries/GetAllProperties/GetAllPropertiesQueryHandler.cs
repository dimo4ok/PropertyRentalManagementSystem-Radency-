using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.PropertyManagement.Models.Property;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Queries.GetAllProperties;

public class GetAllPropertiesQueryHandler(
    IPropertyQueries propertyQueries,
    ILogger<GetAllPropertiesQueryHandler> logger
) : IQueryHandler<GetAllPropertiesQuery, Result<PaginatedModel<PropertyListModel>>>
{
    private readonly IPropertyQueries _propertyQueries = propertyQueries;
    private readonly ILogger<GetAllPropertiesQueryHandler> _logger = logger;

    public async Task<Result<PaginatedModel<PropertyListModel>>> ExecuteAsync(GetAllPropertiesQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get properties request");

        var paginatedProperties = await _propertyQueries.GetAllPaginatedAsync(query.PaginationParams, cancellationToken);
        if (paginatedProperties.TotalItems is 0)
        {
            _logger.LogWarning("No properties found.");
            return Result<PaginatedModel<PropertyListModel>>.Fail(DomainErrors.NotFound(nameof(Property)));
        }

        _logger.LogInformation(
            "Properties retrieved successfully. ReturnedCount: {ReturnedCount}, TotalItems: {TotalItems}, PageNumber: {PageNumber}, PageSize: {PageSize}.",
            paginatedProperties.Items.Count, paginatedProperties.TotalItems, query.PaginationParams.PageNumber,
            query.PaginationParams.PageSize);

        return Result<PaginatedModel<PropertyListModel>>.Success(paginatedProperties);
    }
}