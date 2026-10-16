namespace PropertyRental.Application.PropertyManagement.Models.Unit;

public record CreateUnitModel(
    string UnitNumber,
    int Bedrooms,
    decimal MonthlyRent,
    Guid UnitTypeId);