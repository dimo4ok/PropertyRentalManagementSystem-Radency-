namespace PropertyRental.Domain.Entities.RentalEntities;

public class Residence
{
    public Guid Id { get; set; }

    public string Address { get; set; } = null!;
    public string LandlordName { get; set; } = null!;
    public string LandlordPhone { get; set; } = null!;

    public DateTimeOffset MoveInDate { get; set; }
    public DateTimeOffset? MoveOutDate { get; set; }

    public Guid RentalApplicationId { get; set; }
    public RentalApplication RentalApplication { get; set; } = null!;
}