namespace PropertyRental.Application.RentalManagement.Models.Residence;

public record ResidenceModel(
    Guid Id,
    string Address,
    string LandlordName,
    string LandlordPhone,
    DateTimeOffset MoveInDate,
    DateTimeOffset? MoveOutDate);