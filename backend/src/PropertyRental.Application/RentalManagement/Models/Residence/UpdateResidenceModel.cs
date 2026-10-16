namespace PropertyRental.Application.RentalManagement.Models.Residence;

public record UpdateResidenceModel(
    string Address,
    string LandlordName,
    string LandlordPhone,
    DateTimeOffset MoveInDate,
    DateTimeOffset? MoveOutDate);