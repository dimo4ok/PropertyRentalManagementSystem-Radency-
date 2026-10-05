using Microsoft.AspNetCore.Http;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Services;

public class UnitTypeService(
    IUnitTypeRepository unitTypeRepository)
    : IUnitTypeService
{
    private readonly IUnitTypeRepository _unitTypeRepository = unitTypeRepository;

    public async Task<Result> CheckCanBeUsedAsync(
        IEnumerable<Guid> unitTypeIds,
        CancellationToken cancellationToken)
    {
        var ids = unitTypeIds.Distinct().ToList();
        if (ids.Count == 0)
            return Result.Success();

        var unitTypes = await _unitTypeRepository.GetByIdsAsync(ids, cancellationToken);
        if (unitTypes.Count != ids.Count)
            return Result.Fail(UnitTypeErrors.Invalid, StatusCodes.Status400BadRequest);

        if (unitTypes.Any(x => !x.IsActive))
            return Result.Fail(UnitTypeErrors.Inactive, StatusCodes.Status400BadRequest);

        return Result.Success();
    }
}