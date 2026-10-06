using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.RentalManagement.Interfaces;

public interface IRentalApplicationRepository
{
    Task<RentalApplication?> GetByIdAsync(Guid id, bool track, CancellationToken cancellationToken);
    Task<ICollection<RentalApplication>> GetAllAsync(CancellationToken cancellationToken = default);
}