using PropertyRental.Application.Common.Models;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.RentalManagement.Interfaces;

public interface IRentalApplicationService
{
    Task<Result> ValidateCanBeDeletedAsync(IEnumerable<Unit> units, CancellationToken cancellationToken);
}