using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.Application.RentalManagement.Models.RentalApplication;

public record RentalApplicationListItemModel(
    Guid Id,
    Guid UnitId,
    string UnitNumber,
    Guid PropertyId,
    string PropertyName,
    string FullName,
    string Email,
    ApplicationStatus Status,
    DateTimeOffset CreatedAt);