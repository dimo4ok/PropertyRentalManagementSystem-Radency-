using FitCoachHub.Application.Common.Models;

namespace PropertyRental.Application.Common.Errors;

public static class RentalApplicationErrors
{
    public static Error UnitHasApplications =>
        new(
            "RentalApplication.UnitHasApplications",
            "The unit cannot be deleted because it has rental applications.");
}