using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Domain.Entities.RentalEntities;
using PropertyRental.Infrastructure.Persistence;

namespace PropertyRental.Infrastructure.RentalManagement.Repositories;

public class RentalApplicationRepository(AppDbContext dbContext) : IRentalApplicationRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task<RentalApplication?> GetByIdWithResidencesAndStatusHistoryAsync(Guid id, bool track = false, CancellationToken cancellationToken = default)
    {
        var query = track
            ? _dbContext.RentalApplications
            : _dbContext.RentalApplications.AsNoTracking();

        return await query
            .Include(x => x.ResidenceHistory)
            .Include(x => x.StatusHistory)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<RentalApplication?> GetByIdWithResidencesAsync(Guid id, bool track = false, CancellationToken cancellationToken = default)
    {
        var query = track
            ? _dbContext.RentalApplications
            : _dbContext.RentalApplications.AsNoTracking();

        return await query
            .Include(x => x.ResidenceHistory)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<RentalApplication?> GetByIdWithStatusHistoriesAsync(Guid id, bool track = false, CancellationToken cancellationToken = default)
    {
        var query = track
            ? _dbContext.RentalApplications
            : _dbContext.RentalApplications.AsNoTracking();

        return await query
            .Include(x => x.StatusHistory)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task CreateAsync(RentalApplication rentalApplication, CancellationToken cancellationToken = default)
        => await _dbContext.RentalApplications.AddAsync(rentalApplication, cancellationToken);

    public void SetOriginalRowVersion(RentalApplication rentalApplication, byte[] rowVersion)
        => _dbContext.Entry(rentalApplication)
            .Property(x => x.RowVersion)
            .OriginalValue = rowVersion;
}