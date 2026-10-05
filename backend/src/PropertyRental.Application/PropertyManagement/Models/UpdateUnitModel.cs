namespace PropertyRental.Application.PropertyManagement.Models;

public record UpdateUnitModel(
    Guid Id,
    string UnitNumber,
    int Bedrooms,
    decimal MonthlyRent,
    Guid UnitTypeId);