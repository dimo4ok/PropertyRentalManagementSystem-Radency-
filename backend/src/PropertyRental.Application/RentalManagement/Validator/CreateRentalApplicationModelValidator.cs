using FluentValidation;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;

namespace PropertyRental.Application.RentalManagement.Validator;

public class CreateRentalApplicationModelValidator : AbstractValidator<CreateRentalApplicationModel>
{
    public CreateRentalApplicationModelValidator()
    {
        RuleFor(x => x.UnitId)
            .NotEmpty();

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200);

        RuleFor(x => x.CurrentAddress)
            .NotEmpty()
            .MaximumLength(500);

        RuleForEach(x => x.ResidenceHistory)
            .SetValidator(new CreateResidenceModelValidator());
    }
}