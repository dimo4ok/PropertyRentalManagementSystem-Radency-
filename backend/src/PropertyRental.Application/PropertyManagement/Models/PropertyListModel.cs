namespace PropertyRental.Application.PropertyManagement.Models;

public record PropertyListModel(
    Guid Id,
    string Name,
    string Address);