using PropertyRental.Application.RentalManagement.Models.Residence;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Application.RentalManagement.Extensions;

public static class ResidenceExtensions
{
    public static Residence ToEntity(this UpdateResidenceModel model)
        => new()
        {
            Address = model.Address,
            LandlordName = model.LandlordName,
            LandlordPhone = model.LandlordPhone,
            MoveInDate = model.MoveInDate,
            MoveOutDate = model.MoveOutDate
        };
}