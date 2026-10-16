namespace PropertyRental.Application.PropertyManagement.Models.Property;

public record UpdatePropertyModel(
    string Name,
    string Address,
    string RowVersion);