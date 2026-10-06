using FluentValidation;
using PropertyRental.Application.PropertyManagement.Models.Unit;

namespace PropertyRental.Application.PropertyManagement.Validators.Unit;

public class CreateUnitModelValidator : AbstractValidator<CreateUnitModel>
{
    public CreateUnitModelValidator()
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
    }
}