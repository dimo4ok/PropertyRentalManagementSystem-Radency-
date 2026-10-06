using FluentValidation;
using PropertyRental.Application.PropertyManagement.Models.Unit;

namespace PropertyRental.Application.PropertyManagement.Validators.Unit;

public class UpdateUnitModelValidator : AbstractValidator<UpdateUnitModel>
{
    public UpdateUnitModelValidator()
    {
        RuleFor(x => x.UnitNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Bedrooms)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MonthlyRent)
            .GreaterThan(0);

        RuleFor(x => x.UnitTypeId)
            .NotEmpty();

        RuleFor(x => x.RowVersion)
            .NotEmpty()
            .Must(BeValidBase64)
            .WithMessage("RowVersion must be a valid Base64 string.");
    }

    private static bool BeValidBase64(string value)
    {
        try
        {
            Convert.FromBase64String(value);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}