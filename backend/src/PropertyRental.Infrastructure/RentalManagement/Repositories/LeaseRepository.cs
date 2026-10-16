using PropertyRental.Application.Interfaces.Repositories;
using PropertyRental.Domain.Entities.RentalEntities;
using PropertyRental.Infrastructure.Persistence;

namespace PropertyRental.Infrastructure.RentalManagement.Repositories;

public class LeaseRepository(AppDbContext dbContext) : ILeaseRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task CreateAsync(Lease lease, CancellationToken cancellationToken = default)
        => await _dbContext.Leases.AddAsync(lease, cancellationToken);
}