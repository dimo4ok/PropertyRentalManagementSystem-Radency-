using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Domain.Entities.Identity;

namespace PropertyRental.Domain.Entities.RentalEntities;

public class ApplicationStatusHistory
{
    public Guid Id { get; set; }
    public DateTimeOffset ChangedAt { get; set; }
    public string? Comment { get; set; }

    public ApplicationStatus? FromStatus { get; set; }
    public ApplicationStatus ToStatus { get; set; }

    public Guid RentalApplicationId { get; set; }
    public RentalApplication RentalApplication { get; set; } = null!;

    public Guid ChangedByUserId { get; set; }
    public User ChangedByUser { get; set; } = null!;
}