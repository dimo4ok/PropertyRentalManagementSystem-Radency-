using FluentValidation;
using PropertyRental.Application.PropertyManagement.Models;
using PropertyRental.Application.PropertyManagement.Models.Property;

namespace PropertyRental.Application.PropertyManagement.Validators.Property;

public class CreatePropertyModelValidator : AbstractValidator<CreatePropertyModel>
{
    public CreatePropertyModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(500);
    }
}