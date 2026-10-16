using FitCoachHub.Application.Common.Models;

namespace PropertyRental.Application.Common.Errors;

public static class UnitTypeErrors
{
    public static Error Invalid =>
        new("UnitType.Invalid", "One or more specified unit types do not exist.");

    public static Error Inactive =>
        new("UnitType.Inactive", "One or more specified unit types are inactive.");
}