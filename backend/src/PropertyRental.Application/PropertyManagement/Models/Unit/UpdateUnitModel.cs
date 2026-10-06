namespace PropertyRental.Application.PropertyManagement.Models.Unit;

public record UpdateUnitModel(
    string UnitNumber,
    int Bedrooms,
    decimal MonthlyRent,
    Guid UnitTypeId, 
    string RowVersion);