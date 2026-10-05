using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.RentalManagement.Interfaces;

public interface IRentalApplicationRepository
{
    Task<ICollection<RentalApplication>> GetAllAsync(
        CancellationToken cancellationToken);
}