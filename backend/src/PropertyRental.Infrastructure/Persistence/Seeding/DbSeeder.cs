using Microsoft.AspNetCore.Identity;
using PropertyRental.Domain.Entities.Identity;

namespace PropertyRental.Infrastructure.Persistence.Seeding;

public class DbSeeder(
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager)
{
    private readonly AppDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;

    public async Task SeedAsync()
    {
        await IdentitySeeder.SeedAsync(_roleManager, _userManager);
        await PropertySeeder.SeedAsync(_context);
        await RentalSeeder.SeedAsync(_context);
    }
}