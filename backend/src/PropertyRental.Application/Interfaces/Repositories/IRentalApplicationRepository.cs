using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.Interfaces.Repositories;

public interface IRentalApplicationRepository
{
    Task<RentalApplication?> GetByIdWithResidencesAndStatusHistoryAsync(Guid id, bool track = false, CancellationToken cancellationToken = default);
    Task<RentalApplication?> GetByIdWithResidencesAsync(Guid id, bool track = false, CancellationToken cancellationToken = default);
    Task<RentalApplication?> GetByIdWithStatusHistoriesAsync(Guid id, bool track = false, CancellationToken cancellationToken = default);
    Task CreateAsync(RentalApplication rentalApplication, CancellationToken cancellationToken = default);
    void SetOriginalRowVersion(RentalApplication rentalApplication, byte[] rowVersion);
}