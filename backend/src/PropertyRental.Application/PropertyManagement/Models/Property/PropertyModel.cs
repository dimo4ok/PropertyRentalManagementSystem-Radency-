using PropertyRental.Application.PropertyManagement.Models.Unit;

namespace PropertyRental.Application.PropertyManagement.Models.Property;

public record PropertyModel(
    Guid Id,
    string Name,
    string Address,
    ICollection<UnitModel> Units,
    string RowVersion);