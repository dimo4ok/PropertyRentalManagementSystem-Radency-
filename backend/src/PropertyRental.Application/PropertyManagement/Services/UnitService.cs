using Microsoft.AspNetCore.Http;
using PropertyRental.Application.Common.Errors;
using PropertyRental.Application.Common.Models;
using PropertyRental.Application.PropertyManagement.Interfaces;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.RentalManagement.Interfaces;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Services;

public class UnitService(
    IUnitTypeService unitTypeService,
    IRentalApplicationService rentalApplicationService) : IUnitService
{
    private readonly IUnitTypeService _unitTypeService = unitTypeService;
    private readonly IRentalApplicationService _rentalApplicationService = rentalApplicationService;

    public async Task<Result> UpdateAsync(
        Property property,
        ICollection<UpdateUnitModel> models,
        CancellationToken cancellationToken)
    {
        var existingUnits = property.Units.ToDictionary(x => x.Id);

        var invalidUnit = models
            .Where(x => x.Id != Guid.Empty)
            .FirstOrDefault(x => !existingUnits.ContainsKey(x.Id));

        if (invalidUnit is not null)
            return Result.Fail(DomainErrors.InvalidEntityId(nameof(Unit)), StatusCodes.Status400BadRequest);

        var unitTypeIdsToCheck = models
            .Where(x => x.Id == Guid.Empty || existingUnits[x.Id].UnitTypeId != x.UnitTypeId)
            .Select(x => x.UnitTypeId)
            .Distinct()
            .ToList();

        var unitTypeResult = await _unitTypeService.CheckCanBeUsedAsync(unitTypeIdsToCheck, cancellationToken);
        if (!unitTypeResult.IsSuccess)
            return unitTypeResult;

        var requestedUnitIds = models
            .Where(x => x.Id != Guid.Empty)
            .Select(x => x.Id)
            .ToHashSet();

        var unitsToDelete = property.Units
            .Where(x => !requestedUnitIds.Contains(x.Id))
            .ToList();

        var deleteResult = await DeleteUnitsAsync(property, unitsToDelete, cancellationToken);
        if (!deleteResult.IsSuccess)
            return deleteResult;

        UpdateUnits(existingUnits, models);
        AddUnits(property, models);

        return Result.Success();
    }


    private async Task<Result> DeleteUnitsAsync(Property property, ICollection<Unit> units,
        CancellationToken cancellationToken)
    {
        var result = await _rentalApplicationService.ValidateCanBeDeletedAsync(units, cancellationToken);
        if (!result.IsSuccess)
            return result;

        foreach (var unit in units)
        {
            property.Units.Remove(unit);
        }

        return Result.Success();
    }

    private static void UpdateUnits(Dictionary<Guid, Unit> existingUnits, ICollection<UpdateUnitModel> models)
    {
        foreach (var model in models.Where(x => x.Id != Guid.Empty))
        {
            var unit = existingUnits[model.Id];

            unit.UnitNumber = model.UnitNumber;
            unit.Bedrooms = model.Bedrooms;
            unit.MonthlyRent = model.MonthlyRent;
            unit.UnitTypeId = model.UnitTypeId;
        }
    }

    private static void AddUnits(Property property, ICollection<UpdateUnitModel> models)
    {
        foreach (var model in models.Where(x => x.Id == Guid.Empty))
        {
            property.Units.Add(new Unit
            {
                UnitNumber = model.UnitNumber,
                Bedrooms = model.Bedrooms,
                MonthlyRent = model.MonthlyRent,
                UnitTypeId = model.UnitTypeId
            });
        }
    }
}