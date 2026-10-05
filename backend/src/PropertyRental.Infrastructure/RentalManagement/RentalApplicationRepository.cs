using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.RentalManagement.Interfaces;
using PropertyRental.Domain.Entities.RentalEntities;
using PropertyRental.Infrastructure.Persistence;

namespace PropertyRental.Infrastructure.RentalManagement;

public class RentalApplicationRepository(AppDbContext dbContext) : IRentalApplicationRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<ICollection<RentalApplication>> GetAllAsync(CancellationToken cancellationToken)
        => await _dbContext.RentalApplications
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}