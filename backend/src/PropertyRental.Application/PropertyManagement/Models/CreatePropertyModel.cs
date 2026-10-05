namespace PropertyRental.Application.PropertyManagement.Models;

public record CreatePropertyModel(
    string Name,
    string Address,
    ICollection<CreateUnitModel> Units);