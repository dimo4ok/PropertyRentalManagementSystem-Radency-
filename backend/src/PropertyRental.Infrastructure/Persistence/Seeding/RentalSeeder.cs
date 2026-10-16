using Bogus;
using Microsoft.EntityFrameworkCore;
using PropertyRental.Domain.Entities.Enums;
using PropertyRental.Domain.Entities.Identity;
using PropertyRental.Domain.Entities.RentalEntities;

namespace PropertyRental.Infrastructure.Persistence.Seeding;

public static class RentalSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        var applications = await SeedRentalApplicationsAsync(context);
        if (applications.Count == 0)
            return;

        await SeedResidencesAsync(context, applications);
        await SeedApplicationStatusHistoryAsync(context, applications);
        await SeedLeasesAsync(context, applications);
    }

    private static async Task<List<RentalApplication>> SeedRentalApplicationsAsync(AppDbContext context)
    {
        if (await context.RentalApplications.AnyAsync())
            return [];

        var applicants = await context.Users.Where(x => x.UserName!.StartsWith("applicant")).ToListAsync();
        var units = await context.Units.ToListAsync();
        var statuses = Enum.GetValues<ApplicationStatus>();
        var faker = new Faker();
        var applications = new List<RentalApplication>();

        for (var i = 0; i < statuses.Length; i++)
        {
            var applicant = applicants[i % applicants.Count];
            var unit = units[i % units.Count];

            var application = new RentalApplication
            {
                UnitId = unit.Id, UserId = applicant.Id, Status = statuses[i],
                FullName = $"{applicant.FirstName} {applicant.LastName}", Phone = applicant.PhoneNumber!,
                Email = applicant.Email!, CurrentAddress = faker.Address.FullAddress()
            };
            applications.Add(application);
        }

        await context.RentalApplications.AddRangeAsync(applications);
        await context.SaveChangesAsync();
        return applications;
    }

    private static async Task SeedResidencesAsync(AppDbContext context, List<RentalApplication> applications)
    {
        var faker = new Faker();

        foreach (var application in applications)
        {
            for (var i = 0; i < 2; i++)
            {
                var residence = new Residence
                {
                    RentalApplicationId = application.Id, Address = faker.Address.FullAddress(),
                    LandlordName = faker.Name.FullName(), LandlordPhone = faker.Phone.PhoneNumber(),
                    MoveInDate = DateTime.UtcNow.AddYears(-(i + 2)), MoveOutDate = DateTime.UtcNow.AddYears(-(i + 1))
                };
                context.Residences.Add(residence);
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedApplicationStatusHistoryAsync(AppDbContext context,
        List<RentalApplication> applications)
    {
        var applicants = await context.Users.Where(x => x.UserName!.StartsWith("applicant")).ToListAsync();
        var managers = await context.Users.Where(x => x.UserName!.StartsWith("manager")).ToListAsync();
        var manager = managers[0];

        foreach (var application in applications)
        {
            var applicant = applicants.First(x => x.Id == application.UserId);
            switch (application.Status)
            {
                case ApplicationStatus.Draft:
                    CreateStatusHistory(context, application, null, ApplicationStatus.Draft, applicant); break;
                case ApplicationStatus.Submitted:
                    CreateStatusHistory(context, application, null, ApplicationStatus.Draft, applicant);
                    CreateStatusHistory(context, application, ApplicationStatus.Draft, ApplicationStatus.Submitted,
                        applicant);
                    break;
                case ApplicationStatus.Returned:
                    CreateStatusHistory(context, application, null, ApplicationStatus.Draft, applicant);
                    CreateStatusHistory(context, application, ApplicationStatus.Draft, ApplicationStatus.Submitted,
                        applicant);
                    CreateStatusHistory(context, application, ApplicationStatus.Submitted, ApplicationStatus.Returned,
                        manager, "Please provide additional information.");
                    break;
                case ApplicationStatus.Approved:
                    CreateStatusHistory(context, application, null, ApplicationStatus.Draft, applicant);
                    CreateStatusHistory(context, application, ApplicationStatus.Draft, ApplicationStatus.Submitted,
                        applicant);
                    CreateStatusHistory(context, application, ApplicationStatus.Submitted, ApplicationStatus.Approved,
                        manager);
                    break;
                case ApplicationStatus.Denied:
                    CreateStatusHistory(context, application, null, ApplicationStatus.Draft, applicant);
                    CreateStatusHistory(context, application, ApplicationStatus.Draft, ApplicationStatus.Submitted,
                        applicant);
                    CreateStatusHistory(context, application, ApplicationStatus.Submitted, ApplicationStatus.Denied,
                        manager, "Application does not meet the requirements.");
                    break;
                case ApplicationStatus.Withdrawn:
                    CreateStatusHistory(context, application, null, ApplicationStatus.Draft, applicant);
                    CreateStatusHistory(context, application, ApplicationStatus.Draft, ApplicationStatus.Submitted,
                        applicant);
                    CreateStatusHistory(context, application, ApplicationStatus.Submitted, ApplicationStatus.Withdrawn,
                        applicant);
                    break;
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedLeasesAsync(AppDbContext context, List<RentalApplication> applications)
    {
        var approvedApplications = applications.Where(x => x.Status == ApplicationStatus.Approved).ToList();

        if (approvedApplications.Count == 0)
            return;

        var units = await context.Units.ToListAsync();
        foreach (var application in approvedApplications)
        {
            var unit = units.First(x => x.Id == application.UnitId);

            var lease = new Lease
            {
                UnitId = unit.Id, UserId = application.UserId, StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(12)), MonthlyRent = unit.MonthlyRent,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };
            context.Leases.Add(lease);
        }

        await context.SaveChangesAsync();
    }

    private static void CreateStatusHistory(AppDbContext context, RentalApplication application,
        ApplicationStatus? fromStatus, ApplicationStatus toStatus, User changedBy, string? comment = null)
    {
        var history = new ApplicationStatusHistory
        {
            RentalApplicationId = application.Id, FromStatus = fromStatus, ToStatus = toStatus,
            ChangedAt = DateTime.UtcNow, ChangedByUserId = changedBy.Id, Comment = comment
        };
        context.ApplicationStatusHistories.Add(history);
    }
}