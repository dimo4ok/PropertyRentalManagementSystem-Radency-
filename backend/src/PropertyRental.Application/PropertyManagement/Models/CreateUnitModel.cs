namespace PropertyRental.Application.PropertyManagement.Models;

public record CreateUnitModel(
    string UnitNumber,
    int Bedrooms,
    decimal MonthlyRent,
    Guid UnitTypeId);