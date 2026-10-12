using FitCoachHub.Application.Common.Models;

namespace PropertyRental.Application.Common.Errors;

public static class RentalApplicationErrors
{
    public static Error UnitHasApplications => new(
        "RentalApplication.UnitHasApplications",
        "The unit cannot be deleted because it has rental applications.");

    public static Error UnitNotAvailable => new(
        "RentalApplication.UnitNotAvailable",
        "The selected unit is not available.");

    public static Error CannotEdit => new(
        "RentalApplication.CannotEdit",
        "Rental application cannot be edited in its current status.");

    public static readonly Error OpenApplicationAlreadyExists =
        new(
            "RentalApplication.OpenApplicationAlreadyExists",
            "You already have an open application for this unit.");

    public static readonly Error CannotSubmit =
        new(
            "RentalApplication.CannotSubmit",
            "Rental application cannot be submitted in its current status.");

    public static readonly Error CannotWithdraw =
        new(
            "RentalApplication.CannotWithdraw",
            "Rental application cannot be withdrawn in its current status.");

    public static readonly Error CannotReturn =
        new(
            "RentalApplication.CannotReturn",
            "Rental application cannot be returned in its current status.");

    public static readonly Error CannotDeny =
        new(
            "RentalApplication.CannotDeny",
            "Rental application cannot be denied in its current status.");

    public static readonly Error CannotApprove =
        new(
            "RentalApplication.CannotApprove",
            "Rental application cannot be approved in its current status.");
}