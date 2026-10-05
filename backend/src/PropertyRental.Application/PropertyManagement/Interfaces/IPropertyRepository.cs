using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool track = false);
    IQueryable<Property> GetAllAsync();
    Task CreateAsync(Property property, CancellationToken cancellationToken = default);
    void Delete(Property property);
    void SetOriginalRowVersion(Property property, byte[] rowVersion);
}