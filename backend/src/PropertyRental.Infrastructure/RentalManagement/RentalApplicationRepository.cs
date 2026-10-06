using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.RentalManagement.Interfaces;
using PropertyRental.Domain.Entities.RentalEntities;
using PropertyRental.Infrastructure.Persistence;

namespace PropertyRental.Infrastructure.RentalManagement;

public class RentalApplicationRepository(AppDbContext dbContext) : IRentalApplicationRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<ICollection<RentalApplication>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.RentalApplications
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<RentalApplication?> GetByIdAsync(Guid id, bool track = false, CancellationToken cancellationToken = default)
    {
        var query = track
            ? _dbContext.RentalApplications
            : _dbContext.RentalApplications.AsNoTracking();

        return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}