using FluentValidation;
using PropertyRental.Application.PropertyManagement.Models;

namespace PropertyRental.Application.PropertyManagement.Validators;

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
    }
}