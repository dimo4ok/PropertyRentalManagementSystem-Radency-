using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Application.PropertyManagement.Extensions;

public static class PropertyExtensions
{
    public static PropertyModel ToPropertyModel(this Property entity)
        => new(
            entity.Id,
            entity.Name,
            entity.Address,
            entity.Units.Select(x => x.ToModel()).ToList(),
            Convert.ToBase64String(entity.RowVersion));

    private static UnitTypeModel ToUnitTypeModel(this UnitType entity)
        => new(
            entity.Id,
            entity.Name);

    public static PropertyListModel ToPropertyListModel(this Property entity)
        => new(
            entity.Id,
            entity.Name,
            entity.Address);

    public static Property ToEntity(this CreatePropertyModel model)
        => new()
        {
            Name = model.Name,
            Address = model.Address
        };
}