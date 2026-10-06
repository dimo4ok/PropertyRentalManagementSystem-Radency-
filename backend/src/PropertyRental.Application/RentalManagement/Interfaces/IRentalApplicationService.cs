using PropertyRental.Application.Common.Models;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.RentalManagement.Interfaces;

public interface IRentalApplicationService
{
    Task<Result> ValidateCanBeDeletedAsync(Guid unitId, CancellationToken cancellationToken);
    Task<Result> ValidateCanBeDeletedAsync(IEnumerable<Guid> unitIds, CancellationToken cancellationToken);
    
}