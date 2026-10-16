using FitCoachHub.Application.Common.Models;

namespace PropertyRental.Application.Common.Errors;

public static class UnitErrors
{
    public static readonly Error UnitHasActiveLease =
        new(
            "Unit.UnitHasActiveLease",
            "The unit cannot be updated because it has an active lease.");

    public static readonly Error UnitHasSubmittedOrApprovedApplication =
        new(
            "Unit.UnitHasSubmittedOrApprovedApplication",
            "The unit cannot be updated because it has a submitted or approved application.");
}