using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Domain.Entities.PropertyEntities;

public class Unit
{
    public Guid Id { get; set; }
    public string UnitNumber { get; set; } = null!;
    public int Bedrooms { get; set; }
    public decimal MonthlyRent { get; set; }

    public Guid PropertyId { get; set; }
    public Property Property { get; set; } = null!;

    public Guid UnitTypeId { get; set; }
    public UnitType UnitType { get; set; } = null!;

    public ICollection<Lease> Leases { get; set; } = [];

    public byte[] RowVersion { get; set; } = [];
}