namespace PropertyRental.Application.PropertyManagement.Models;

public record UnitModel(
    Guid Id,
    string UnitNumber,
    int Bedrooms,
    decimal MonthlyRent,
    UnitTypeModel UnitType);