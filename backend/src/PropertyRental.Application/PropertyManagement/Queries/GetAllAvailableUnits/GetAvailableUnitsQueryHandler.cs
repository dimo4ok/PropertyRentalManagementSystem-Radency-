using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.PropertyManagement.Models.Unit;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Queries.GetAllAvailableUnits;

public class GetAvailableUnitsQueryHandler(IUnitQueries unitQueries)
    : IQueryHandler<GetAvailableUnitsQuery, Result<PaginatedModel<UnitListModel>>>
{
    private readonly IUnitQueries _unitQueries = unitQueries;

    public async Task<Result<PaginatedModel<UnitListModel>>> ExecuteAsync(GetAvailableUnitsQuery query,
        CancellationToken cancellationToken)
    {
        var availablePaginatedUnits = await _unitQueries.GetAvailableUnitsPaginatedAsync(query.PaginationParams, cancellationToken);
        if (availablePaginatedUnits.TotalItems is 0)
            return Result<PaginatedModel<UnitListModel>>.Fail(DomainErrors.NotFound(nameof(Unit)));

        return Result<PaginatedModel<UnitListModel>>.Success(availablePaginatedUnits);
    }
}