using Microsoft.AspNetCore.Identity;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Domain.Entities.Identity;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public ICollection<RentalApplication> Applications { get; set; } = [];
}