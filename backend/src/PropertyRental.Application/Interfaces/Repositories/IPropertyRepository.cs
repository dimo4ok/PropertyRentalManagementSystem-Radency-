using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.Interfaces.Repositories;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(Guid id, bool track = false, CancellationToken cancellationToken = default);
    Task<Property?> GetByIdWithUnitsAndUnitTypesAsync( Guid id, bool track = false, CancellationToken cancellationToken = default);
    Task CreateAsync(Property property, CancellationToken cancellationToken = default);
    void Delete(Property property);
    void SetOriginalRowVersion(Property property, byte[] rowVersion);
}