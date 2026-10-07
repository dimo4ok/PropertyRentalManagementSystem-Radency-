using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Interfaces;

public interface IUnitService
{
    IQueryable<Unit> GetAvailableUnitsByQuery();
}