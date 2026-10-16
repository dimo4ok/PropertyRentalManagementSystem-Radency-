using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.PropertyManagement.Models.Property;

namespace PropertyRental.Application.Interfaces.Queries;

public interface IPropertyQueries
{
    Task<PaginatedModel<PropertyListModel>> GetAllPaginatedAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string name, string address, CancellationToken cancellationToken, Guid? excludePropertyId = null);
}