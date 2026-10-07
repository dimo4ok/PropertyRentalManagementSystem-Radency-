using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Extensions;
using PropertyRental.Application.Common.Mediator.Abstractions;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.PropertyManagement.Extensions;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Application.PropertyManagement.Models.Unit;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Queries.GetAllAvailableUnits;

public class GetAvailableUnitsQueryHandler(
    IUnitService unitService)
    : IQueryHandler<GetAvailableUnitsQuery, Result<PaginatedModel<UnitListModel>>>
{
    private readonly IUnitService _unitService = unitService;

    public async Task<Result<PaginatedModel<UnitListModel>>> ExecuteAsync(GetAvailableUnitsQuery query,
        CancellationToken cancellationToken)
    {
        var availableUnitsQuery = _unitService.GetAvailableUnitsByQuery();
        if (!await availableUnitsQuery.AnyAsync(cancellationToken))
            return Result<PaginatedModel<UnitListModel>>.Fail(DomainErrors.NotFound(nameof(Unit)));

        var paginated = await availableUnitsQuery.PaginateAsync(query.PaginationParams.PageNumber,
            query.PaginationParams.PageSize, cancellationToken);

        return Result<PaginatedModel<UnitListModel>>.Success(paginated.ToModel(x => x.ToUnitListModel()));
    }
}