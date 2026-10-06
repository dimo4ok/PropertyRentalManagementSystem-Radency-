using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Domain.Entities.PropertyEntities;
using PropertyRental.Infrastructure.Persistence;

namespace PropertyRental.Infrastructure.PropertyManagement;

public class UnitRepository(AppDbContext dbContext) : IUnitRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Unit?> GetByIdAsync(Guid id, bool track = false, CancellationToken cancellationToken = default)
    {
        var query = track
            ? _dbContext.Units
            : _dbContext.Units.AsNoTracking();

        return await query
            .Include(x => x.UnitType)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task CreateAsync(Unit unit, CancellationToken cancellationToken = default)
        => await _dbContext.Units.AddAsync(unit, cancellationToken);

    public void Delete(Unit unit)
        => _dbContext.Units.Remove(unit);

    public void SetOriginalRowVersion(Unit unit, byte[] rowVersion)
        => _dbContext.Entry(unit)
            .Property(x => x.RowVersion)
            .OriginalValue = rowVersion;
}