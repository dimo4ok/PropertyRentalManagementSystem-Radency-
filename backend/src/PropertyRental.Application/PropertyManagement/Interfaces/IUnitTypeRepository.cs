using PropertyRental.Application.Common.Models;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Interfaces;

public interface IUnitTypeRepository
{
    Task<List<UnitType>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}