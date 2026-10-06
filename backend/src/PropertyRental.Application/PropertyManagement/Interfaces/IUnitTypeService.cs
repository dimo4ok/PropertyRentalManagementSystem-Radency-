using PropertyRental.Application.Common.Models;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Interfaces;

public interface IUnitTypeService
{
    Task<Result> ValidateAsync(
        Guid unitTypeId,
        CancellationToken cancellationToken);
}