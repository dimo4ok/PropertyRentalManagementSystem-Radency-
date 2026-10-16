using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.PropertyManagement.Models.Unit;

namespace PropertyRental.Application.Interfaces.Queries;

public interface IUnitQueries
{
    Task<PaginatedModel<UnitListModel>> GetAvailableUnitsPaginatedAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
    Task<bool> IsAvailableAsync(Guid unitId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync( Guid propertyId, string unitNumber, CancellationToken cancellationToken, Guid? excludeUnitId = null);
}