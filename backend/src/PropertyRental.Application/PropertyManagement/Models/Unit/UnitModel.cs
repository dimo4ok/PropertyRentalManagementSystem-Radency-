namespace PropertyRental.Application.PropertyManagement.Models.Unit;

public record UnitModel(
    Guid Id,
    string UnitNumber,
    int Bedrooms,
    decimal MonthlyRent,
    UnitTypeModel UnitType,
    string RowVersion);