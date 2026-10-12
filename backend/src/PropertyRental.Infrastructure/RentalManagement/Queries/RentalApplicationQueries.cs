using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.Common.Models.Pagination;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;
using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Infrastructure.Extensions;
using PropertyRental.Infrastructure.Persistence;

namespace PropertyRental.Infrastructure.RentalManagement.Queries;

public class RentalApplicationQueries(AppDbContext dbContext) : IRentalApplicationQueries
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<PaginatedModel<RentalApplicationListItemModel>> GetAllFilteredAndPaginatedAsync(
        ApplicationStatus? status, Guid? propertyId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _dbContext.RentalApplications
            .AsNoTracking()
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        if (propertyId.HasValue)
            query = query.Where(x => x.Unit.PropertyId == propertyId.Value);

        var modelsQuery = query
            .OrderByDescending(x => x.CreatedAt)
            .ThenBy(x => x.Id)
            .Select(x => new RentalApplicationListItemModel(
                x.Id,
                x.UnitId,
                x.Unit.UnitNumber,
                x.Unit.PropertyId,
                x.Unit.Property.Name,
                x.FullName,
                x.Email,
                x.Status,
                x.CreatedAt));

        return await modelsQuery.PaginateAsync(pageNumber, pageSize, cancellationToken);
    }

    public async Task<PaginatedModel<RentalApplicationListItemModel>> GetAllForApplicantFilteredAndPaginatedAsync(
        Guid userId, ApplicationStatus? status, Guid? propertyId, int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.RentalApplications
            .AsNoTracking()
            .Where(x => x.UserId == userId);

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        if (propertyId.HasValue)
            query = query.Where(x => x.Unit.PropertyId == propertyId.Value);

        var modelsQuery = query
            .OrderByDescending(x => x.CreatedAt)
            .ThenBy(x => x.Id)
            .Select(x => new RentalApplicationListItemModel(
                x.Id,
                x.UnitId,
                x.Unit.UnitNumber,
                x.Unit.PropertyId,
                x.Unit.Property.Name,
                x.FullName,
                x.Email,
                x.Status,
                x.CreatedAt));

        return await modelsQuery.PaginateAsync(pageNumber, pageSize, cancellationToken);
    }

    public Task<bool> HasApplicationForUnitAsync(
        Guid unitId,
        CancellationToken cancellationToken)
        => _dbContext.RentalApplications
            .AnyAsync(x => x.UnitId == unitId, cancellationToken);

    public Task<bool> HasApplicationForAnyUnitAsync(
        IEnumerable<Guid> unitIds,
        CancellationToken cancellationToken)
        => _dbContext.RentalApplications
            .AnyAsync(x => unitIds.Contains(x.UnitId), cancellationToken);

    public Task<bool> HasOpenApplicationForUnitAsync(
        Guid userId,
        Guid unitId,
        CancellationToken cancellationToken)
        => _dbContext.RentalApplications
            .AnyAsync(x =>
                    x.UserId == userId &&
                    x.UnitId == unitId &&
                    (x.Status == ApplicationStatus.Draft ||
                     x.Status == ApplicationStatus.Submitted ||
                     x.Status == ApplicationStatus.Returned),
                cancellationToken);
    
    public Task<bool> HasSubmittedOrApprovedApplicationForUnitAsync(Guid unitId, CancellationToken cancellationToken)
        => _dbContext.RentalApplications
            .AnyAsync(x =>
                    x.UnitId == unitId &&
                    (x.Status == ApplicationStatus.Submitted ||
                     x.Status == ApplicationStatus.Approved),
                cancellationToken);
}