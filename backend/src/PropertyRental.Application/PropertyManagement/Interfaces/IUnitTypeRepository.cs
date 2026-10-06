using PropertyRental.Application.Common.Models;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Interfaces;

public interface IUnitTypeRepository
{
    Task<UnitType?> GetByIdAsync(Guid id, bool track = false, CancellationToken cancellationToken = default);
    Task<List<UnitType>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}