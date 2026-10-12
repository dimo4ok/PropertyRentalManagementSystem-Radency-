using PropertyRental.Application.RentalManagement.Models.Residence;
using PropertyRental.Domain.Entities.Enums;

namespace PropertyRental.Application.RentalManagement.Models.RentalApplication;

public record ApplicantRentalApplicationModel(
    Guid Id,
    Guid UnitId,
    string FullName,
    string Phone,
    string Email,
    string CurrentAddress,
    ApplicationStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    ICollection<ResidenceModel> ResidenceHistory);