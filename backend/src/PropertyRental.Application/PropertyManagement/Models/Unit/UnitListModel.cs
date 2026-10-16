using PropertyRental.Application.PropertyManagement.Models.UnitType;

namespace PropertyRental.Application.PropertyManagement.Models.Unit;

public record UnitListModel(
    Guid Id,
    string UnitNumber,
    int Bedrooms,
    decimal MonthlyRent,
    UnitTypeModel UnitType);