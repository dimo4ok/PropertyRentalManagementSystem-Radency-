using Bogus;
using Microsoft.AspNetCore.Identity;
using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Domain.Entities.Identity;

namespace PropertyRental.Infrastructure.Persistence.Seeding;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<User> userManager)
    {
        await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager);
    }

    private static async Task SeedRolesAsync(
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        var roles = new[]
        {
            UserRole.Applicant.ToString(),
            UserRole.PropertyManager.ToString()
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = role
                });
            }
        }
    }

    private static async Task SeedUsersAsync(
        UserManager<User> userManager)
    {
        var faker = new Faker();

        for (var i = 1; i <= 2; i++)
        {
            var user = new User
            {
                UserName = $"manager{i}@test.com",
                Email = $"manager{i}@test.com",
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                PhoneNumber = faker.Phone.PhoneNumber()
            };

            await CreateUserAsync(
                userManager,
                user,
                UserRole.PropertyManager.ToString());
        }

        for (var i = 1; i <= 5; i++)
        {
            var user = new User
            {
                UserName = $"applicant{i}@test.com",
                Email = $"applicant{i}@test.com",
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                PhoneNumber = faker.Phone.PhoneNumber()
            };

            await CreateUserAsync(
                userManager,
                user,
                UserRole.Applicant.ToString());
        }
    }

    private static async Task CreateUserAsync(
        UserManager<User> userManager,
        User user,
        string role)
    {
        if (await userManager.FindByEmailAsync(user.Email!) is not null)
        {
            return;
        }

        var result = await userManager.CreateAsync(user, "Password123!");

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                $"Failed to create seed user: " +
                $"{string.Join(", ", result.Errors.Select(x => x.Description))}");
        }

        await userManager.AddToRoleAsync(user, role);
    }
}