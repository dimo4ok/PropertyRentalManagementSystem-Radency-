using FluentValidation;
using PropertyRental.Application.PropertyManagement.Models;

namespace PropertyRental.Application.PropertyManagement.Validators;

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

        RuleFor(x => x.Units)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage("At least one unit is required.");

        RuleForEach(x => x.Units)
            .SetValidator(new CreateUnitModelValidator());
    }
}