using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.Interfaces.Repositories;

public interface IUnitRepository
{
    Task<Unit?> GetByIdWithUnitTypesAsync(Guid id, bool track = false, CancellationToken cancellationToken = default);
    Task<Unit?> GetByIdWithUpdateLockAsync(Guid unitId, CancellationToken cancellationToken);
    Task CreateAsync(Unit unit, CancellationToken cancellationToken = default);
    void Delete(Unit unit);
    void SetOriginalRowVersion(Unit unit, byte[] rowVersion);
}