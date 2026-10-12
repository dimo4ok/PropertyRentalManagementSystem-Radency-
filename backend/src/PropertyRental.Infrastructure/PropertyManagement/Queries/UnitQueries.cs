using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.PropertyManagement.Extensions;
using PropertyRental.Application.PropertyManagement.Models.Unit;
using PropertyRental.Infrastructure.Extensions;
using PropertyRental.Infrastructure.Persistence;

namespace PropertyRental.Infrastructure.PropertyManagement.Queries;

public class UnitQueries(AppDbContext dbContext) : IUnitQueries
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<PaginatedModel<UnitListModel>> GetAvailableUnitsPaginatedAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var query = _dbContext.Units
            .Where(x => x.UnitType.IsActive)
            .Where(x => !x.Leases.Any(lease =>
                lease.StartDate <= today &&
                lease.EndDate >= today))
            .Select(x => x.ToUnitListModel());

        return await query.PaginateAsync(paginationParams.PageNumber, paginationParams.PageSize, cancellationToken);
    }

    public async Task<bool> IsAvailableAsync(Guid unitId, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return await _dbContext.Units
            .AnyAsync(x =>
                    x.Id == unitId &&
                    x.UnitType.IsActive &&
                    !x.Leases.Any(lease =>
                        lease.StartDate <= today &&
                        lease.EndDate >= today),
                cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid propertyId, string unitNumber, CancellationToken cancellationToken, Guid? excludeUnitId = null)
        => await _dbContext.Units
            .AnyAsync(
                unit => unit.PropertyId == propertyId &&
                        unit.UnitNumber == unitNumber &&
                        (!excludeUnitId.HasValue ||
                         unit.Id != excludeUnitId.Value),
                cancellationToken);
}