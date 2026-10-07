using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.PropertyManagement.Models.Unit;
using PropertyRental.Application.PropertyManagement.Models.UnitType;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Extensions;

public static class UnitExtensions
{
    public static Unit ToEntity(
        this CreateUnitModel model,
        Guid propertyId)
        => new()
        {
            PropertyId = propertyId,
            UnitNumber = model.UnitNumber,
            Bedrooms = model.Bedrooms,
            MonthlyRent = model.MonthlyRent,
            UnitTypeId = model.UnitTypeId
        };

    public static UnitModel ToModel(this Unit entity)
        => new(
            entity.Id,
            entity.UnitNumber,
            entity.Bedrooms,
            entity.MonthlyRent,
            new UnitTypeModel(
                entity.UnitType.Id,
                entity.UnitType.Name),
            Convert.ToBase64String(entity.RowVersion));

    public static UnitListModel ToUnitListModel(this Unit entity)
        => new(
            entity.Id,
            entity.UnitNumber,
            entity.Bedrooms,
            entity.MonthlyRent,
            new UnitTypeModel(
                entity.UnitType.Id,
                entity.UnitType.Name));
}