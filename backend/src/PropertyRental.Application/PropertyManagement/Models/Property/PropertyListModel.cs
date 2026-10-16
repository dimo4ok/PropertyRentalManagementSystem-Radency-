namespace PropertyRental.Application.PropertyManagement.Models.Property;

public record PropertyListModel(
    Guid Id,
    string Name,
    string Address);