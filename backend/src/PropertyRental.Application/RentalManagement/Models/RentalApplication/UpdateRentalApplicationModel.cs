using PropertyRental.Application.RentalManagement.Models.Residence;

namespace PropertyRental.Application.RentalManagement.Models.RentalApplication;

public record UpdateRentalApplicationModel(
    string FullName,
    string Phone,
    string Email,
    string CurrentAddress,
    ICollection<UpdateResidenceModel> ResidenceHistory);