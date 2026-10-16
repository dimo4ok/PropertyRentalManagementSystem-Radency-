using FluentValidation;
using PropertyRental.Application.Common.Validator;
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
            .ValidRowVersion();
    }
}