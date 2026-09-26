using PropertyRental.Domain.Entities.Identity;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Domain.Entities.RentalEntities;

public class Lease
{
    public Guid Id { get; set; }

    public Guid? UnitId { get; set; }
    public Unit? Unit { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal MonthlyRent { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.Now;
}