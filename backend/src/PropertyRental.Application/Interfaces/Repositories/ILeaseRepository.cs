using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.Interfaces.Repositories;

public interface ILeaseRepository
{
    Task CreateAsync(Lease lease, CancellationToken cancellationToken = default);
}