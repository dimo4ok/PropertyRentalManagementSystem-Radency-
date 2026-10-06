using Microsoft.AspNetCore.Http;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Services;

public class UnitTypeService(IUnitTypeRepository unitTypeRepository) : IUnitTypeService
{
    private readonly IUnitTypeRepository _unitTypeRepository = unitTypeRepository;

    public async Task<Result> ValidateAsync(Guid unitTypeId, CancellationToken cancellationToken)
    {
        var unitType = await _unitTypeRepository.GetByIdAsync(unitTypeId, false, cancellationToken);
        if (unitType is null)
            return Result.Fail(UnitTypeErrors.Invalid, StatusCodes.Status400BadRequest);

        if (!unitType.IsActive)
            return Result.Fail(UnitTypeErrors.Inactive, StatusCodes.Status400BadRequest);

        return Result.Success();
    }
}