using PropertyRental.Application.Common.Models;

namespace PropertyRental.Application.Interfaces.Services;

public interface IUnitTypeService
{
    Task<Result> ValidateAsync(
        Guid unitTypeId,
        CancellationToken cancellationToken);
}