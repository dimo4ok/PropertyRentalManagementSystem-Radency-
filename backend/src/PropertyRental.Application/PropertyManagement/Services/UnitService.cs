using Microsoft.EntityFrameworkCore;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Services;

public class UnitService(IUnitRepository unitRepository) : IUnitService
{
    private readonly IUnitRepository _unitRepository = unitRepository;

    public IQueryable<Unit> GetAvailableUnitsByQuery()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return _unitRepository
            .GetAllByQuery()
            .Where(x => x.UnitType.IsActive)
            .Where(x => !x.Leases.Any(lease =>
                lease.StartDate <= today &&
                lease.EndDate >= today));
    }
}