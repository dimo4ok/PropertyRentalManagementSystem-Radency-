using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Domain.Entities.PropertyEntities;
using PropertyRental.Infrastructure.Persistence;

namespace PropertyRental.Infrastructure.PropertyManagement;

public class PropertyRepository(AppDbContext dbContext) : IPropertyRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Property?> GetByIdAsync(Guid id, bool track = false,
        CancellationToken cancellationToken = default)
    {
        var query = track
            ? _dbContext.Properties
            : _dbContext.Properties.AsNoTracking();

        return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Property?> GetByIdWithUnitsAsync(Guid id, bool track = false,
        CancellationToken cancellationToken = default)
    {
        var query = track
            ? _dbContext.Properties
            : _dbContext.Properties.AsNoTracking();

        return await query
            .Include(x => x.Units)
            .ThenInclude(x => x.UnitType)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public IQueryable<Property> GetAllAsync()
        => _dbContext.Properties.AsNoTracking();

    public async Task CreateAsync(Property property, CancellationToken cancellationToken = default) =>
        await _dbContext.Properties.AddAsync(property, cancellationToken);

    public void Delete(Property property)
        => _dbContext.Properties.Remove(property);

    public void SetOriginalRowVersion(Property property, byte[] rowVersion)
        => _dbContext.Entry(property)
            .Property(x => x.RowVersion)
            .OriginalValue = rowVersion;
}