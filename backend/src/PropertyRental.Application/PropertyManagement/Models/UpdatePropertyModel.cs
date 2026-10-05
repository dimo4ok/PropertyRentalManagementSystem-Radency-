namespace PropertyRental.Application.PropertyManagement.Models;

public record UpdatePropertyModel(
    Guid Id,
    string Name,
    string Address,
    ICollection<UpdateUnitModel> Units,
    string RowVersion);