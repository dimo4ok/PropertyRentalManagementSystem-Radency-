using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Domain.Entities.Identity;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Domain.Entities.RentalEntities;

public class RentalApplication
{
    public Guid Id { get; set; }

    public Guid UnitId { get; set; }
    public Unit Unit { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;

    public string FullName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string CurrentAddress { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<Residence> ResidenceHistory { get; set; } = [];
}