using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Interfaces;

public interface IUnitRepository
{
    Task<Unit?> GetByIdAsync(Guid id, bool track = false, CancellationToken cancellationToken = default);
    Task CreateAsync(Unit unit, CancellationToken cancellationToken = default);
    void Delete(Unit unit);
    void SetOriginalRowVersion(Unit unit, byte[] rowVersion);
}