using System.Diagnostics;
using Bogus;
using Microsoft.EntityFrameworkCore;
using PropertyRental.Domain.Entities.PropertyEntities;

namespace PropertyRental.Infrastructure.Persistence.Seeding;

public static class PropertySeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedUnitTypesAsync(context);
        await SeedPropertiesAsync(context);
        await SeedUnitsAsync(context);
    }

    private static async Task SeedUnitTypesAsync(AppDbContext context)
    {
        if (await context.UnitTypes.AnyAsync())
        {
            return;
        }

        var unitTypes = new[]
        {
            new UnitType
            {
                Name = "Apartment",
                IsActive = true
            },
            new UnitType
            {
                Name = "Condo",
                IsActive = true
            },
            new UnitType
            {
                Name = "Townhouse",
                IsActive = true
            },
            new UnitType
            {
                Name = "Studio",
                IsActive = false
            }
        };

        await context.UnitTypes.AddRangeAsync(unitTypes);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPropertiesAsync(AppDbContext context)
    {
        if (await context.Properties.AnyAsync())
        {
            return;
        }

        var faker = new Faker<Property>()
            .RuleFor(x => x.Name, f => $"{f.Address.City()} Apartments")
            .RuleFor(x => x.Address, f => f.Address.FullAddress());

        var properties = faker.Generate(3);

        await context.Properties.AddRangeAsync(properties);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUnitsAsync(AppDbContext context)
    {
        if (await context.Units.AnyAsync())
        {
            return;
        }

        var properties = await context.Properties.ToListAsync();
        var unitTypes = await context.UnitTypes
            .Where(x => x.IsActive)
            .ToListAsync();

        var faker = new Faker();

        var units = new List<Unit>();

        foreach (var property in properties)
        {
            for (var i = 1; i <= 3; i++)
            {
                var unitType = faker.PickRandom(unitTypes);

                units.Add(new Unit
                {
                    UnitNumber = $"{i}0{i}",
                    Bedrooms = faker.Random.Int(1, 4),
                    MonthlyRent = faker.Finance.Amount(500, 2500),
                    PropertyId = property.Id,
                    UnitTypeId = unitType.Id
                });
            }
        }

        await context.Units.AddRangeAsync(units);
        await context.SaveChangesAsync();
    }
}