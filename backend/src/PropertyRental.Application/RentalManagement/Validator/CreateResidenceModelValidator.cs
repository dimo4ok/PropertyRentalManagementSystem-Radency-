using FluentValidation;
using PropertyRental.Application.RentalManagement.Models.Residence;

namespace PropertyRental.Application.RentalManagement.Validator;

public class CreateResidenceModelValidator : AbstractValidator<CreateResidenceModel>
{
    public CreateResidenceModelValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.LandlordName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.LandlordPhone)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.MoveOutDate)
            .GreaterThanOrEqualTo(x => x.MoveInDate)
            .When(x => x.MoveOutDate.HasValue);
    }
}