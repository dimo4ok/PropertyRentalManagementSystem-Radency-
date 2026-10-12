using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;
using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.Application.Interfaces.Queries;

public interface IRentalApplicationQueries
{
    Task<PaginatedModel<RentalApplicationListItemModel>> GetAllFilteredAndPaginatedAsync(ApplicationStatus? status,
        Guid? propertyId, int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<PaginatedModel<RentalApplicationListItemModel>> GetAllForApplicantFilteredAndPaginatedAsync(
        Guid userId, ApplicationStatus? status, Guid? propertyId, int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<bool> HasApplicationForUnitAsync(Guid unitId, CancellationToken cancellationToken);
    Task<bool> HasApplicationForAnyUnitAsync(IEnumerable<Guid> unitIds, CancellationToken cancellationToken);
    Task<bool> HasOpenApplicationForUnitAsync(Guid userId, Guid unitId, CancellationToken cancellationToken);
    Task<bool> HasSubmittedOrApprovedApplicationForUnitAsync(Guid unitId, CancellationToken cancellationToken);

}