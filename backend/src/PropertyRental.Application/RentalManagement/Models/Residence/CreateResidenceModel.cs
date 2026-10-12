namespace PropertyRental.Application.RentalManagement.Models.Residence;

public record CreateResidenceModel(
    string Address,
    string LandlordName,
    string LandlordPhone,
    DateTimeOffset MoveInDate,
    DateTimeOffset? MoveOutDate);