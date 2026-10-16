using FluentValidation;
using PropertyRental.Application.Common.Validator;
using PropertyRental.Application.RentalManagement.Models.RentalApplication;

namespace PropertyRental.Application.RentalManagement.Validator;

public class RentalApplicationReviewModelValidator : AbstractValidator<RentalApplicationReviewModel>
{
    public RentalApplicationReviewModelValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.RowVersion)
            .ValidRowVersion();
    }
}