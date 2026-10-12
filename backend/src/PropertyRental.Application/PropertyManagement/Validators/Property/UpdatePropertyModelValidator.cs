using FluentValidation;
using PropertyRental.Application.Common.Validator;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.PropertyManagement.Models.Property;

namespace PropertyRental.Application.PropertyManagement.Validators.Property;

public class UpdatePropertyModelValidator : AbstractValidator<UpdatePropertyModel>
{
    public UpdatePropertyModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.RowVersion)
            .ValidRowVersion();
    }
}