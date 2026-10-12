using FluentValidation;
using PropertyRental.Application.Common.Validator;
using PropertyRental.Application.PropertyManagement.Models.Unit;

namespace PropertyRental.Application.PropertyManagement.Validators.Unit;

public class DeleteUnitModelValidator : AbstractValidator<DeleteUnitModel>
{
    public DeleteUnitModelValidator()
    {
        RuleFor(x => x.RowVersion)
            .ValidRowVersion();
    }
}