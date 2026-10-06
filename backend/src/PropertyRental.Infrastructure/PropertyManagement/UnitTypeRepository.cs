using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Domain.Entities.PropertyEntities;
using PropertyRental.Infrastructure.Persistence;

namespace PropertyRental.Infrastructure.PropertyManagement;

public class UnitTypeRepository(AppDbContext dbContext) : IUnitTypeRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<UnitType>> GetByIdsAsync(IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
        => await _dbContext.UnitTypes
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

    public async Task<UnitType?> GetByIdAsync(Guid id, bool track = false, CancellationToken cancellationToken = default)
    {
        var query = track
            ? _dbContext.UnitTypes
            : _dbContext.UnitTypes.AsNoTracking();

        return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}