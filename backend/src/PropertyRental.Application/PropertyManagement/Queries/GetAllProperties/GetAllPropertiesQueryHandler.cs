using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Extensions;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.PropertyManagement.Extensions;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Queries.GetAllProperties;

public class GetAllPropertiesQueryHandler(
    IPropertyRepository propertyRepository,
    ILogger<GetAllPropertiesQueryHandler> logger
) : IQueryHandler<GetAllPropertiesQuery, Result<PaginatedModel<PropertyListModel>>>
{
    private readonly IPropertyRepository _propertyRepository = propertyRepository;
    private readonly ILogger<GetAllPropertiesQueryHandler> _logger = logger;

    public async Task<Result<PaginatedModel<PropertyListModel>>> ExecuteAsync(GetAllPropertiesQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get properties request");

        var queryProperties = _propertyRepository.GetAllAsync();
        if (!await queryProperties.AnyAsync(cancellationToken))
        {
            _logger.LogWarning("No properties found.");
            return Result<PaginatedModel<PropertyListModel>>.Fail(DomainErrors.NotFound(nameof(Property)));
        }

        var paginated =
            await queryProperties.PaginateAsync(query.Params.PageNumber, query.Params.PageSize, cancellationToken);

        _logger.LogInformation(
            "Properties retrieved successfully. ReturnedCount: {ReturnedCount}, TotalItems: {TotalItems}, PageNumber: {PageNumber}, PageSize: {PageSize}.",
            paginated.Items.Count, paginated.TotalItems, query.Params.PageNumber, query.Params.PageSize);

        return Result<PaginatedModel<PropertyListModel>>.Success(paginated.ToModel(x => x.ToPropertyListModel()));
    }
}