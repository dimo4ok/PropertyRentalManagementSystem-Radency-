using PropertyRental.Application.RentalManagement.Models.Residence;

namespace PropertyRental.Application.RentalManagement.Models.RentalApplication;

public record CreateRentalApplicationModel(
    Guid UnitId,
    string FullName,
    string Phone,
    string Email,
    string CurrentAddress,
    ICollection<CreateResidenceModel> ResidenceHistory);