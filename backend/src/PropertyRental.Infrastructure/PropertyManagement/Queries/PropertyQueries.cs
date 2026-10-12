using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.PropertyManagement.Extensions;
using PropertyRental.Application.PropertyManagement.Models.Property;
using PropertyRental.Infrastructure.Extensions;
using PropertyRental.Infrastructure.Persistence;

namespace PropertyRental.Infrastructure.PropertyManagement.Queries;

public class PropertyQueries(AppDbContext dbContext) : IPropertyQueries
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<PaginatedModel<PropertyListModel>> GetAllPaginatedAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        var query = _dbContext.Properties.Select(x => x.ToPropertyListModel());
        
        return await query.PaginateAsync(paginationParams.PageNumber, paginationParams.PageSize, cancellationToken);
    }
}
