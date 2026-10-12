using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.Interfaces.Queries;
using PropertyRental.Infrastructure.Persistence;

namespace PropertyRental.Infrastructure.RentalManagement.Queries;

public class LeaseQueries(AppDbContext dbContext) : ILeaseQueries
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<bool> HasActiveLeaseAsync(Guid unitId, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return await _dbContext.Leases
            .AnyAsync(x =>
                    x.UnitId == unitId &&
                    x.StartDate <= today &&
                    x.EndDate >= today,
                cancellationToken);
    }
}