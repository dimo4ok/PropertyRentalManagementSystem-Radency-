namespace PropertyRental.Application.PropertyManagement.Models;

public record PropertyModel(
    Guid Id,
    string Name,
    string Address,
    ICollection<UnitModel> Units,
    string RowVersion);