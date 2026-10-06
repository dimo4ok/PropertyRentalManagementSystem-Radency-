using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(Guid id, bool track = false, CancellationToken cancellationToken = default);
    Task<Property?> GetByIdWithUnitsAsync( Guid id, bool track = false, CancellationToken cancellationToken = default);
    IQueryable<Property> GetAllAsync();
    Task CreateAsync(Property property, CancellationToken cancellationToken = default);
    void Delete(Property property);
    void SetOriginalRowVersion(Property property, byte[] rowVersion);
}